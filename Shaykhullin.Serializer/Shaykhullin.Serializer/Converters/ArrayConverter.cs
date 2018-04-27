using System;
using System.IO;

namespace Shaykhullin.Serializer.Core
{
	internal class ArrayConverter : Converter<Array>
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

		public override void Serialize(Stream stream, Array elements)
		{
			var elementType = elements.GetType().GetElementType();

			stream.Write(BitConverter.GetBytes(elements.Length), 0, 4);

			foreach (var element in elements)
			{
				serializer.Serialize(stream, element, elementType);
			}
		}

		public override object DeserializeObject(Stream stream, Type type)
		{
			var elementType = type.GetElementType();

			var lengthBuffer = new byte[4];
			stream.Read(lengthBuffer, 0, 4);
			var length = BitConverter.ToInt32(lengthBuffer, 0);

			var array = Array.CreateInstance(elementType, length);

			for (int i = 0; i < length; i++)
			{
				array.SetValue(serializer.Deserialize(stream, elementType), i);
			}

			return array;
		}
	}
}
