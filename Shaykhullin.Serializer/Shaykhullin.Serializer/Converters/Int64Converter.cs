using System;
using System.IO;

namespace Shaykhullin.Serializer.Core
{
	internal class Int64Converter : Converter<long>
	{
		public override void Serialize(Stream stream, long data)
		{
			var union = new ByteUnion(data);
			stream.WriteByte(union.Byte1);
			stream.WriteByte(union.Byte2);
			stream.WriteByte(union.Byte3);
			stream.WriteByte(union.Byte4);
			stream.WriteByte(union.Byte5);
			stream.WriteByte(union.Byte6);
			stream.WriteByte(union.Byte7);
			stream.WriteByte(union.Byte8);
		}

		public override long Deserialize(Stream stream)
		{
			return new ByteUnion(
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte()).Int64;
		}
	}
}
