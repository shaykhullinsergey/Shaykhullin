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
			
			var union = new UnifiedUnion(elements.Length);
			stream.WriteByte(union.Byte1);
			stream.WriteByte(union.Byte2);
			stream.WriteByte(union.Byte3);
			stream.WriteByte(union.Byte4);

			for (var i = 0; i < elements.Length; i++)
			{
				serializer.Serialize(stream, elements.GetValue(i), elementType);
			}
		}

		public override object DeserializeObject(Stream stream, Type type)
		{
			var elementType = type.GetElementType();

			var length = new UnifiedUnion(
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte()).Int32;
			
			var array = Array.CreateInstance(elementType, length);

			for (var i = 0; i < length; i++)
			{
				array.SetValue(serializer.Deserialize(stream, elementType), i);
			}

			return array;
		}
	}
}
