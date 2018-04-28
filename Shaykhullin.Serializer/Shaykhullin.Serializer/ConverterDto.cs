using System;
using Shaykhullin.Serializer.Core;

namespace Shaykhullin.Serializer.Core
{
	internal class ConverterDto
	{
		public int EntityAlias { get; }
		public Type EntityType { get; }
		public Type ConverterType { get; set; }
		public IConverter Converter { get; set; }

		public ConverterDto(Type entityType)
		{
			EntityType = entityType;
			EntityAlias = GetHash(entityType.Name);
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
