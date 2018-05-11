using System;
using System.Collections.Generic;
using Shaykhullin.DependencyInjection;

namespace Shaykhullin.Serializer.Core
{
	public class AliasDto
	{
		public int AliasHash { get; }
		public Type AliasType { get; }

		public AliasDto(int aliasHash, Type aliasType)
		{
			AliasHash = aliasHash;
			AliasType = aliasType;
		}
	}
	
	internal class ConverterContainer
	{
		private readonly IContainerConfig config;
		private readonly ConverterContainer parent;
		private readonly List<AliasDto> aliases;
		private readonly Dictionary<Type, ConverterDto> converters;

		public ConverterContainer(IContainerConfig config)
		{
			this.config = config;
			aliases = new List<AliasDto>();
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
				var dto = new ConverterDto(type);
				
				aliases.Add(new AliasDto(dto.EntityAlias, dto.EntityType));
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
				aliases.Add(new AliasDto(dto.EntityAlias, dto.EntityType));
			}
		}

		public Type GetTypeFromAlias(int alias)
		{
			for (var i = 0; i < aliases.Count; i++)
			{
				if (aliases[i].AliasHash == alias)
				{
					return aliases[i].AliasType;
				}
			}

			return parent?.GetTypeFromAlias(alias);
		}

		public ConverterDto TryGetDto(Type type)
		{
			var converterDto = TryGetDtoRecursive(type);

			if (converterDto != null)
			{
				if (converterDto.Converter == null && converterDto.ConverterType != null)
				{
					using (var container = config.Create())
					{
						converterDto.Converter = (IConverter)container.Resolve(converterDto.ConverterType);
					}
				}
			}

			return converterDto;
		}

		private ConverterDto TryGetDtoRecursive(Type type)
		{
			converters.TryGetValue(type, out var converterDto);

			if (converterDto == null)
			{
				foreach (var pair in converters)
				{
					if (pair.Key.IsAssignableFrom(type))
					{
						if (pair.Value.ConverterType == null)
						{
							break;
						}

						converterDto = pair.Value;
						break;
					}
				}
			}

			return converterDto ?? parent?.TryGetDtoRecursive(type);
		}
	}
}
