using System;
using System.Collections.Generic;
using Shaykhullin.DependencyInjection;

namespace Shaykhullin.Serializer.Core
{
	internal class ConverterContainer
	{
		private readonly IContainerConfig config;
		private readonly ConverterContainer parent;
		private readonly Dictionary<Type, ConverterDto> converters;

		public ConverterContainer(IContainerConfig config)
		{
			this.config = config;
			converters = new Dictionary<Type, ConverterDto>();
		}

		internal ConverterContainer(IContainerConfig config, ConverterContainer parent) 
			: this(config)
		{
			this.parent = parent;
		}

		public void RegisterTypeWithAlias(Type type)
		{
			if (TryGetDto(type) == null)
			{
				converters.Add(type, new ConverterDto(type));
			}
		}

		public void RegisterConverterTypeFor(Type type, Type converterType)
		{
			if (!converters.ContainsKey(type))
			{
				converters.Add(type, new ConverterDto(type)
				{
					ConverterType = converterType
				});
			}
			else
			{
				converters[type].ConverterType = converterType;
			}
		}

		public Type GetTypeFromAlias(int alias)
		{
			foreach (var element in converters)
			{
				if(element.Value.Alias == alias)
				{
					return element.Key;
				}
			}

			return parent?.GetTypeFromAlias(alias);
		}

		public ConverterDto TryGetDto(Type type)
		{
			if (converters.TryGetValue(type, out var dto))
			{
				if (dto.Converter == null && dto.ConverterType != null)
				{
					using (var container = config.Create())
					{
						dto.Converter = (IConverter)container.Resolve(dto.ConverterType);
					}
				}

				return dto;
			}

			foreach (var pair in converters)
			{
				if (pair.Key.IsAssignableFrom(type))
				{
					if (pair.Value.ConverterType == null)
					{
						break;
					}

					if (pair.Value.Converter == null)
					{
						using (var container = config.Create())
						{
							pair.Value.Converter = (IConverter)container.Resolve(pair.Value.ConverterType);
						}
					}

					return pair.Value;
				}
			}

			return parent?.TryGetDto(type);
		}
	}
}
