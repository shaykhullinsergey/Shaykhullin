using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Shaykhullin.Activator;
using Shaykhullin.DependencyInjection;

namespace Shaykhullin.Serializer
{
	internal class Serializer : ISerializer
	{
		private readonly IContainer container;
		private readonly Configuration configuration;
		private readonly Dictionary<Type, PropertyInfo[]> properties;

		public Serializer(IContainerConfig config, Configuration configuration)
		{
			config.Register<ISerializer>()
				.ImplementedBy(c => this)
				.As<Singleton>();

			this.container = config.Container;
			this.configuration = configuration;
			properties = new Dictionary<Type, PropertyInfo[]>();
		}

		public void Serialize<TData>(Stream stream, TData data)
		{
			Serialize(stream, (object)data, null);
		}

		public TData Deserialize<TData>(Stream stream)
		 {
			return (TData)Deserialize(stream, typeof(TData));
		}

		public void Serialize(Stream stream, object data, Type dataTypeOverride)
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

			if (configuration.TryGetValue(type, out var dto))
			{
				if(dto.Converter != null)
				{
					dto.Converter.SerializeObject(stream, data);
					return;
				}
				else
				{
					var aliasBytes = BitConverter.GetBytes(dto.Alias);
					stream.Write(aliasBytes, 0, aliasBytes.Length);
				}
			}
			else
			{
				type = dataTypeOverride;
				if(configuration.TryGetValue(type, out var dtoOverride))
				{
					if (dtoOverride.Converter != null)
					{
						dto.Converter.SerializeObject(stream, data);
						return;
					}
					else
					{
						var aliasBytes = BitConverter.GetBytes(dtoOverride.Alias);
						stream.Write(aliasBytes, 0, aliasBytes.Length);
					}
				}
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

			if (configuration.TryGetValue(type, out var dto))
			{
				if(dto.Converter != null)
				{
					return dto.Converter.DeserializeObject(stream, type);
				}
				else
				{
					var aliasBytes = new byte[4];
					stream.Read(aliasBytes, 0, 4);
					var alias = BitConverter.ToInt32(aliasBytes, 0);
					type = configuration.GetTypeFromAlias(alias) ?? type;
				}
			}
			else
			{

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
