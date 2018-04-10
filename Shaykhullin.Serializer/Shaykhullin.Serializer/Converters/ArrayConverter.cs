using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Shaykhullin.Serializer.Core
{
	public class ArrayConverter : Converter<Array>
	{
		private readonly ISerializer serializer;
		private readonly Dictionary<Type, int> arrayTypes = new Dictionary<Type, int>();

		public ArrayConverter(ISerializer serializer)
		{
			this.serializer = serializer;
		}

		public override Array Deserialize(Stream stream)
		{
			var lengthBuffer = new byte[4];
			stream.Read(lengthBuffer, 0, 4);
			var length = BitConverter.ToInt32(lengthBuffer, 0);

			var elementIdBuffer = new byte[4];
			stream.Read(elementIdBuffer, 0, 4);
			var elementId = BitConverter.ToInt32(elementIdBuffer, 0);

			var elementType = arrayTypes.First(x => x.Value == elementId).Key;
			var array = Array.CreateInstance(elementType, length);

			var idBuffer = new byte[4];
			for (int i = 0; i < length; i++)
			{
				stream.Read(idBuffer, 0, 4);
				var id = BitConverter.ToInt32(idBuffer, 0);

				foreach (var pair in arrayTypes)
				{
					if(pair.Value == id)
					{
						array.SetValue(serializer.Deserialize(stream, pair.Key), i);
					}
				}
			}

			return array;
		}

		public override void Serialize(Stream stream, Array data)
		{
			stream.Write(BitConverter.GetBytes(data.Length), 0, 4);

			var elementType = data.GetType().GetElementType();

			if (!arrayTypes.TryGetValue(elementType, out var id))
			{
				id = GetHash(elementType.Name);
				arrayTypes.Add(elementType, id);
			}

			stream.Write(BitConverter.GetBytes(id), 0, 4);

			foreach (var element in data)
			{
				elementType = element.GetType();

				if (!arrayTypes.TryGetValue(elementType, out id))
				{
					id = GetHash(elementType.Name);
					arrayTypes.Add(elementType, id);
				}

				stream.Write(BitConverter.GetBytes(id), 0, 4);
				serializer.Serialize(stream, element);
			}
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
