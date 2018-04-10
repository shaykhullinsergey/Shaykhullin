using System;
using Shaykhullin.Serializer.Core;

namespace Shaykhullin.Serializer
{
	public class ConverterDto
	{
		public Type Type { get; set; }
		public IConverter Converter { get; set; }
	}
}
