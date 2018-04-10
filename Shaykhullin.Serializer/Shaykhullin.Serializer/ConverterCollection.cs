using System;
using System.Collections.Generic;
using Shaykhullin.DependencyInjection;
using Shaykhullin.Serializer.Core;

namespace Shaykhullin.Serializer
{
	public class ConverterCollection
	{
		private readonly IContainer container;
		private readonly Dictionary<Type, ConverterDto> converters;

		public ConverterCollection(IContainer container)
		{
			this.container = container;
			converters = new Dictionary<Type, ConverterDto>();
		}

		public void Add(Type type, Type converterType, IContainerConfig config)
		{
			var dto = new ConverterDto
			{
				Type = converterType
			};

			if (converters.ContainsKey(type))
			{
				converters[type] = dto;
			}
			else
			{
				converters.Add(type, dto);
			}

			config.Register(converterType)
				.As<Singleton>();
		}

		public bool TryGetValue(Type type, out IConverter converter)
		{
			if(converters.TryGetValue(type, out var dto))
			{
				if (dto.Converter == null)
				{
					dto.Converter = (IConverter)container.Resolve(dto.Type);
				}

				converter = dto.Converter;
				return true;
			}

			foreach (var pair in converters)
			{
				if(pair.Key.IsAssignableFrom(type))
				{
					if(pair.Value.Converter == null)
					{
						pair.Value.Converter = (IConverter)container.Resolve(pair.Value.Type);
					}

					converter = pair.Value.Converter;
					return true;
				}
			}

			converter = null;
			return false;
		}
	}
}
