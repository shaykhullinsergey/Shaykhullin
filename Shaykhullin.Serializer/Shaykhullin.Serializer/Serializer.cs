﻿using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Shaykhullin.Activator;
using Shaykhullin.Stream;

namespace Shaykhullin.Serializer.Core
{
	internal class Serializer : ISerializer
	{
		private static BindingFlags PublicInstanceBindingFlags = BindingFlags.Public | BindingFlags.Instance;
		
		private bool disposed;
		private readonly IActivator activator;
		private readonly ConverterContainer converterContainer;
		private readonly Dictionary<Type, PropertyInfo[]> properties;

		public Serializer(IActivator activator, ConverterContainer converterContainer)
		{
			this.activator = activator;
			this.converterContainer = converterContainer;
			properties = new Dictionary<Type, PropertyInfo[]>();
		}

		public void Serialize<TData>(ValueStream stream, TData data)
		{
			if (disposed)
			{
				throw new ObjectDisposedException(nameof(Serializer));
			}

			Serialize(stream, data, typeof(TData));
		}

		public TData Deserialize<TData>(ValueStream stream)
		{
			if (disposed)
			{
				throw new ObjectDisposedException(nameof(Serializer));
			}
			
			return (TData)Deserialize(stream, typeof(TData));
		}

		public void Serialize(ValueStream stream, object data, Type dataTypeOverride)
		{
			if (disposed)
			{
				throw new ObjectDisposedException(nameof(Serializer));
			}

			if (dataTypeOverride == null)
			{
				throw new ArgumentNullException(nameof(dataTypeOverride));
			}

			if (data == null)
			{
				stream.WriteByte(0);
				return;
			}

			var dataType = data.GetType();

			if (!dataType.IsValueType || Nullable.GetUnderlyingType(dataType) != null)
			{
				stream.WriteByte(1);
			}

			var dto = converterContainer.TryGetDto(dataType);

			if (dto == null)
			{
				dataType = dataTypeOverride;
				dto = converterContainer.TryGetDto(dataTypeOverride);
			}
			else
			{
				if(dto.Converter != null)
				{
					dto.Converter.SerializeObject(stream, data);
					return;
				}
			}

			if(dto == null)
			{
				stream.WriteByte(0);
			}
			else
			{
				stream.WriteByte(1);
				
				var union = new UnifiedUnion(dto.EntityAlias);
				stream.WriteByte(union.Byte1);
				stream.WriteByte(union.Byte2);
				stream.WriteByte(union.Byte3);
				stream.WriteByte(union.Byte4);
			}

			foreach (var property in EnsureProperties(dataType))
			{
				Serialize(stream, property.GetValue(data), property.PropertyType);
			}
		}

		public object Deserialize(ValueStream stream, Type dataType)
		{
			if (disposed)
			{
				throw new ObjectDisposedException(nameof(Serializer));
			}
			
			if (dataType == null)
			{
				throw new ArgumentNullException(nameof(dataType));
			}
			
			if (!dataType.IsValueType || Nullable.GetUnderlyingType(dataType) != null)
			{
				if (stream.ReadByte() == 0)
				{
					return null;
				}
			}

			var dto = converterContainer.TryGetDto(dataType);

			if (dto?.Converter != null)
			{
				return dto.Converter.DeserializeObject(stream, dataType);
			}

			if (stream.ReadByte() == 1)
			{
				var aliasBytes = new byte[4];
				stream.Read(aliasBytes, 0, aliasBytes.Length);
				
				var alias = new UnifiedUnion(aliasBytes[0], aliasBytes[1], aliasBytes[2], aliasBytes[3]).Int32;
				
				dataType = converterContainer.GetTypeFromAlias(alias);
			}
			
			var instance = activator.Create(dataType);

			var propertyInfo = EnsureProperties(dataType);
			for (var i = 0; i < propertyInfo.Length; i++)
			{
				propertyInfo[i].SetValue(instance, Deserialize(stream, propertyInfo[i].PropertyType));
			}

			return instance;
		}

		private PropertyInfo[] EnsureProperties(Type type)
		{
			if (disposed)
			{
				throw new ObjectDisposedException(nameof(Serializer));
			}
			
			if (type == null)
			{
				throw new ArgumentNullException(nameof(type));
			}
			
			if (!properties.TryGetValue(type, out var propertiesInfo))
			{
				propertiesInfo = type
					.GetProperties(PublicInstanceBindingFlags)
					.Where(x => x.CanRead && x.CanWrite)
					.ToArray();

				properties.Add(type, propertiesInfo);
			}

			return propertiesInfo;
		}

		public void Dispose()
		{
			if (!disposed)
			{
				properties.Clear();
				disposed = true;
			}
		}
	}
}