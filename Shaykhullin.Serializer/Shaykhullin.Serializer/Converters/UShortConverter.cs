using System;
using System.IO;

namespace Shaykhullin.Serializer.Core
{
	internal class UShortConverter : Converter<ushort>
	{
		public override void Serialize(Stream stream, ushort data)
		{
			var union = new UnifiedUnion(data);
			stream.WriteByte(union.Byte1);
			stream.WriteByte(union.Byte2);
		}

		public override ushort Deserialize(Stream stream)
		{
			return new UnifiedUnion(
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte()).UInt16;
		}
	}
}
