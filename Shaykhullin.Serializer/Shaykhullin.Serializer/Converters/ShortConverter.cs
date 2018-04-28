using System;
using System.IO;

namespace Shaykhullin.Serializer.Core
{
	internal class ShortConverter : Converter<short>
	{
		public override void Serialize(Stream stream, short data)
		{
			var union = new ByteUnion(data);
			stream.WriteByte(union.Byte1);
			stream.WriteByte(union.Byte2);
		}

		public override short Deserialize(Stream stream)
		{
			return new ByteUnion(
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte()).Int16;
		}
	}
}
