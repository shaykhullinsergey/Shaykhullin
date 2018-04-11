using System;
using System.Collections.Generic;
using Shaykhullin.DependencyInjection;

namespace Shaykhullin.Serializer.Core
{
	internal class Configuration
	{
		private readonly IContainer container;
		private readonly Dictionary<Type, ConverterDto> converters;

		public Configuration(IContainer container)
		{
			this.container = container;
			converters = new Dictionary<Type, ConverterDto>();
		}

		public void RegisterTypeWithAlias(Type type)
		{
			converters.Add(type, new ConverterDto(type));
		}

		public void RegisterConverterTypeFor(Type type, Type converterType)
		{
			converters[type].ConverterType = converterType;
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

			return null;
		}

		public int GetAliasFromType(Type type)
		{
			foreach (var element in converters)
			{
				if (element.Value.Type == type)
				{
					return element.Value.Alias;
				}
			}

			throw new EntryPointNotFoundException();
		}

		public bool TryGetValue(Type type, out ConverterDto dto)
		{
			if(converters.TryGetValue(type, out dto))
			{
				if (dto.Converter == null && dto.ConverterType != null)
				{
					dto.Converter = (IConverter)container.Resolve(dto.ConverterType);
				}

				return true;
			}

			foreach (var pair in converters)
			{
				if(pair.Key.IsAssignableFrom(type))
				{
					if(pair.Value.ConverterType == null)
					{
						break;
					}

					if(pair.Value.Converter == null)
					{
						pair.Value.Converter = (IConverter)container.Resolve(pair.Value.ConverterType);
					}

					dto = pair.Value;
					return true;
				}
			}

			dto = null;
			return false;
		}
	}
}
