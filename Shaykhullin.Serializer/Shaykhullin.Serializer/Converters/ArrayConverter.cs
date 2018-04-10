using System;
using System.IO;

namespace Shaykhullin.Serializer.Core
{
	public class ArrayConverter : Converter<Array>
	{
		private readonly ISerializer serializer;

		public ArrayConverter(ISerializer serializer)
		{
			this.serializer = serializer;
		}

		public override Array Deserialize(Stream stream)
		{
			throw new InvalidOperationException();
		}

		public override void Serialize(Stream stream, Array data)
		{
			stream.Write(BitConverter.GetBytes(data.Length), 0, 4);

			foreach (var element in data)
			{
				serializer.Serialize(stream, element);
			}
		}

		public override object DeserializeObject(Stream stream, Type type)
		{
			var lengthBuffer = new byte[4];
			stream.Read(lengthBuffer, 0, 4);
			var length = BitConverter.ToInt32(lengthBuffer, 0);

			var elementType = type.GetElementType();
			var array = Array.CreateInstance(elementType, length);

			for (int i = 0; i < length; i++)
			{
				array.SetValue(serializer.Deserialize(stream, elementType), i);
			}

			return array;
		}
	}
}
