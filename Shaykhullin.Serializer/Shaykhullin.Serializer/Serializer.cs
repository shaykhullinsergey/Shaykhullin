using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Shaykhullin.Activator;
using Shaykhullin.Serializer.Core;

namespace Shaykhullin.Serializer
{
	internal class Serializer : ISerializer
	{
		private readonly IActivator activator;
		private readonly Dictionary<Type, IConverter> converters;
		private readonly Dictionary<Type, PropertyInfo[]> properties;

		public Serializer(IActivator activator, Dictionary<Type, IConverter> converters)
		{
			this.activator = activator;
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

			if (converters.TryGetValue(dataType, out var converter))
			{
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

			var instance = activator.Create(type);

			foreach (var p in props)
			{
				p.SetValue(instance, Deserialize(stream, p.PropertyType));
			}

			return instance;
		}
	}
}
