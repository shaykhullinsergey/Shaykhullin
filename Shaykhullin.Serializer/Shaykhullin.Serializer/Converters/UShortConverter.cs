using System;
using System.IO;
using Shaykhullin.Stream;

namespace Shaykhullin.Serializer.Core
{
	internal class UShortConverter : Converter<ushort>
	{
		public override void Serialize(ValueStream stream, ushort data)
		{
			var union = new UnifiedUnion(data);
			stream.WriteByte(union.Byte1);
			stream.WriteByte(union.Byte2);
		}

		public override ushort Deserialize(ValueStream stream)
		{
			return new UnifiedUnion(
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte()).UInt16;
		}
	}
}
