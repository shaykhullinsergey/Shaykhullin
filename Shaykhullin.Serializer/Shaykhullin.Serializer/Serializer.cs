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

		public Serializer(IActivator activator, Dictionary<Type, IConverter> converters)
		{
			this.activator = activator;
			this.converters = converters;
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

			var props = dataType
				.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Where(x => x.CanRead && x.CanWrite)
				.ToArray();

			foreach (var p in props)
			{
				var value = p.GetValue(data);

				Serialize(stream, value);
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

			var props = type
				.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Where(x => x.CanRead && x.CanWrite)
				.ToArray();

			var instance = activator.Create(type);

			foreach (var p in props)
			{
				p.SetValue(instance, Deserialize(stream, p.PropertyType));
			}

			return instance;
		}
	}
}
