using System;
using Shaykhullin.Serializer.Core;

namespace Shaykhullin.Serializer
{
	public class ConverterDto
	{
		public int Alias { get; }
		public Type Type { get;  }
		public Type ConverterType { get; set; }
		public IConverter Converter { get; set; }

		public ConverterDto(Type type)
		{
			Type = type;
			Alias = GetHash(type.Name);
		}

		private static unsafe int GetHash(string name)
		{
			fixed (char* str = name)
			{
				var chPtr = str;
				var num = 352654597;
				var num2 = num;
				var numPtr = (int*)chPtr;
				for (var i = name.Length; i > 0; i -= 4)
				{
					num = (num << 5) + num + (num >> 27) ^ numPtr[0];
					if (i <= 2)
					{
						break;
					}
					num2 = (num2 << 5) + num2 + (num2 >> 27) ^ numPtr[1];
					numPtr += 2;
				}
				return num + num2 * 1566083941;
			}
		}
	}
}
