using System;
using System.IO;

namespace Shaykhullin.Serializer.Core
{
	internal class UInt64Converter : Converter<ulong>
	{
		public override void Serialize(Stream stream, ulong data)
		{
			var union = new UnifiedUnion(data);
			stream.WriteByte(union.Byte1);
			stream.WriteByte(union.Byte2);
			stream.WriteByte(union.Byte3);
			stream.WriteByte(union.Byte4);
			stream.WriteByte(union.Byte5);
			stream.WriteByte(union.Byte6);
			stream.WriteByte(union.Byte7);
			stream.WriteByte(union.Byte8);
		}

		public override ulong Deserialize(Stream stream)
		{
			return new UnifiedUnion(
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte()).UInt64;
		}
	}
}
