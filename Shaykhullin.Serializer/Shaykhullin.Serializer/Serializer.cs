using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Shaykhullin.DependencyInjection;
using Shaykhullin.Serializer.Core;
using Shaykhullin.Activator;

namespace Shaykhullin.Serializer
{
	internal class Serializer : ISerializer
	{
		private readonly IContainer container;
		private readonly ConverterCollection converters;
		private readonly Dictionary<Type, PropertyInfo[]> properties;

		public Serializer(IContainerConfig config, ConverterCollection converters)
		{
			config.Register<ISerializer>()
				.ImplementedBy(c => this)
				.As<Singleton>();

			this.container = config.Container;
			this.converters = converters;
			properties = new Dictionary<Type, PropertyInfo[]>();
		}

		public void Serialize<TData>(Stream stream, TData data)
		{
			Serialize(stream, (object)data);
		}

		public TData Deserialize<TData>(Stream stream)
		{
			return (TData)Deserialize(stream, typeof(TData));
		}

		public void Serialize(Stream stream, object data)
		{
			if(data == null)
			{
				stream.WriteByte(1);
				return;
			}

			var type = data.GetType();

			if (!type.IsValueType || (Nullable.GetUnderlyingType(type) != null))
			{
				stream.WriteByte(0);
			}

			if (converters.TryGetValue(type, out var converter))
			{
				converter.SerializeObject(stream, data);
				return;
			}

			if (!properties.TryGetValue(type, out var props))
			{
				props = type
					.GetProperties(BindingFlags.Public | BindingFlags.Instance)
					.Where(x => x.CanRead && x.CanWrite)
					.ToArray();
				properties.Add(type, props);
			}

			foreach (var p in props)
			{
				Serialize(stream, p.GetValue(data));
			}
		}

		public object Deserialize(Stream stream, Type type)
		{
			if (!type.IsValueType || (Nullable.GetUnderlyingType(type) != null))
			{
				if (stream.ReadByte() == 1)
				{
					return null;
				}
			}

			if (converters.TryGetValue(type, out var converter))
			{
				return converter.DeserializeObject(stream);
			}

			if (!properties.TryGetValue(type, out var props))
			{
				props = type
					.GetProperties(BindingFlags.Public | BindingFlags.Instance)
					.Where(x => x.CanRead && x.CanWrite)
					.ToArray();
				properties.Add(type, props);
			}

			var instance = container.Resolve<IActivator>().Create(type);

			foreach (var p in props)
			{
				p.SetValue(instance, Deserialize(stream, p.PropertyType));
			}

			return instance;
		}
	}
}
