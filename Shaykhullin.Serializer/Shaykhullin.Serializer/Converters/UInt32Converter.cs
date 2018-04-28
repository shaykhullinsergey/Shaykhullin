using System.IO;

namespace Shaykhullin.Serializer.Core
{
	internal class UInt32Converter : Converter<uint>
	{
		public override void Serialize(Stream stream, uint data)
		{
			var union = new ByteUnion(data);
			stream.WriteByte(union.Byte1);
			stream.WriteByte(union.Byte2);
			stream.WriteByte(union.Byte3);
			stream.WriteByte(union.Byte4);
		}

		public override uint Deserialize(Stream stream)
		{
			return new ByteUnion(
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte()).UInt32;
		}
	}
}
