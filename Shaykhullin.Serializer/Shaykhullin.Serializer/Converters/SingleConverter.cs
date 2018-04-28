using System;
using System.IO;

namespace Shaykhullin.Serializer.Core
{
	internal class SingleConverter : Converter<float>
	{
		public override void Serialize(Stream stream, float data)
		{
			var union = new ByteUnion(data);
			stream.WriteByte(union.Byte1);
			stream.WriteByte(union.Byte2);
			stream.WriteByte(union.Byte3);
			stream.WriteByte(union.Byte4);
		}

		public override float Deserialize(Stream stream)
		{
			return new ByteUnion(
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte()).Single;
		}
	}
}
