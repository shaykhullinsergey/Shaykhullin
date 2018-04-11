using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Shaykhullin.Activator;
using Shaykhullin.DependencyInjection;

namespace Shaykhullin.Serializer.Core
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

			container = config.Container;
			this.configuration = configuration;
			properties = new Dictionary<Type, PropertyInfo[]>();
		}

		public void Serialize<TData>(Stream stream, TData data)
		{
			Serialize(stream, data, typeof(TData));
		}

		public TData Deserialize<TData>(Stream stream)
		{
			return (TData)Deserialize(stream, typeof(TData));
		}

		public void Serialize(Stream stream, object data, Type dataTypeOverride)
		{
			if (stream == null)
			{
				throw new ArgumentNullException(nameof(stream));
			}

			if (data == null)
			{
				stream.WriteByte(1);
				return;
			}

			var dataType = data.GetType();

			if (!dataType.IsValueType || (Nullable.GetUnderlyingType(dataType) != null))
			{
				stream.WriteByte(0);
			}

			var dto = configuration.TryGetDto(dataType);

			if (dto == null)
			{
				dataType = dataTypeOverride;
				dto = configuration.TryGetDto(dataTypeOverride);
			}

			if (dto?.Converter != null)
			{
				dto.Converter.SerializeObject(stream, data);
				return;
			}

			stream.Write(BitConverter.GetBytes(dto?.Alias ?? 0), 0, 4);

			foreach (var property in EnsureProperties(dataType))
			{
				Serialize(stream, property.GetValue(data), property.PropertyType);
			}
		}

		public object Deserialize(Stream stream, Type dataType)
		{
			if (stream == null)
			{
				throw new ArgumentNullException(nameof(stream));
			}

			if (!dataType.IsValueType || (Nullable.GetUnderlyingType(dataType) != null))
			{
				if (stream.ReadByte() == 1)
				{
					return null;
				}
			}

			var dto = configuration.TryGetDto(dataType);
			if (dto?.Converter != null)
			{
				return dto.Converter.DeserializeObject(stream, dataType);
			}

			var aliasBytes = new byte[4];
			stream.Read(aliasBytes, 0, aliasBytes.Length);
			var alias = BitConverter.ToInt32(aliasBytes, 0);
			dataType = configuration.GetTypeFromAlias(alias) ?? dataType;

			var instance = container.Resolve<IActivator>().Create(dataType);

			foreach (var propertyInfo in EnsureProperties(dataType))
			{
				propertyInfo.SetValue(instance, Deserialize(stream, propertyInfo.PropertyType));
			}

			return instance;
		}

		private PropertyInfo[] EnsureProperties(Type type)
		{
			if (!properties.TryGetValue(type, out var propertiesInfo))
			{
				propertiesInfo = type
					.GetProperties(BindingFlags.Public | BindingFlags.Instance)
					.Where(x => x.CanRead && x.CanWrite)
					.ToArray();

				properties.Add(type, propertiesInfo);
			}

			return propertiesInfo;
		}
	}
}
