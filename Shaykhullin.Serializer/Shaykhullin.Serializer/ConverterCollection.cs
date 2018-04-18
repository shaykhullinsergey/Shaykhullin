using System;
using System.Collections.Generic;
using Shaykhullin.DependencyInjection;

namespace Shaykhullin.Serializer.Core
{
	internal class ConverterContainer
	{
		private readonly IContainerConfig config;
		private readonly ConverterContainer parent;
		private readonly Dictionary<int, Type> aliases;
		private readonly Dictionary<Type, ConverterDto> converters;

		public ConverterContainer(IContainerConfig config)
		{
			this.config = config;
			converters = new Dictionary<Type, ConverterDto>();
			aliases = new Dictionary<int, Type>();
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
				var dto = new ConverterDto(type);
				aliases.Add(dto.Alias, dto.Type);
				converters.Add(type, dto);
			}
		}

		public void RegisterConverterTypeFor(Type type, Type converterType)
		{
			if (converters.TryGetValue(type, out var dto))
			{
				dto.ConverterType = converterType;
			}
			else
			{
				converters.Add(type, new ConverterDto(type)
				{
					ConverterType = converterType
				});
				aliases.Add(dto.Alias, dto.Type);
			}
		}

		public Type GetTypeFromAlias(int alias)
		{
			return aliases.TryGetValue(alias, out var type) 
				? type 
				: parent?.GetTypeFromAlias(alias);
		}

		public ConverterDto TryGetDto(Type type)
		{
			// Try find from current collection
			if (converters.TryGetValue(type, out var currentDto))
			{
				if (currentDto.Converter == null && currentDto.ConverterType != null)
				{
					using (var container = config.Create())
					{
						currentDto.Converter = (IConverter)container.Resolve(currentDto.ConverterType);
					}
				}

				return currentDto;
			}
			
			// Or search for assignment
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
