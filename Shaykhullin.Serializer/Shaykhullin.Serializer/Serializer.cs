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

		public Serializer(IContainer container, ConverterCollection converters)
		{
			this.container = container;
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
			if (data == null)
			{
				stream.WriteByte(1);
				return;
			}

			stream.WriteByte(0);

			var dataType = data.GetType();

			if (converters.TryGetValue(dataType, out var converterType))
			{
				var converter = (IConverter)container.Resolve(converterType);
				converter.SerializeObject(stream, data);
				return;
			}

			if (!properties.TryGetValue(dataType, out var props))
			{
				props = dataType
					.GetProperties(BindingFlags.Public | BindingFlags.Instance)
					.Where(x => x.CanRead && x.CanWrite)
					.ToArray();
				properties.Add(dataType, props);
			}

			foreach (var p in props)
			{
				Serialize(stream, p.GetValue(data));
			}
		}

		public object Deserialize(Stream stream, Type type)
		{
			if (stream.ReadByte() == 1)
			{
				return null;
			}

			if (converters.TryGetValue(type, out var converterType))
			{
				var converter = (IConverter)container.Resolve(converterType);
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
