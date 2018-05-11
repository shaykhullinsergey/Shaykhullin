using System;
using System.IO;
using Shaykhullin.Stream;

namespace Shaykhullin.Serializer.Core
{
	internal class ShortConverter : Converter<short>
	{
		public override void Serialize(ValueStream stream, short data)
		{
			var union = new UnifiedUnion(data);
			stream.WriteByte(union.Byte1);
			stream.WriteByte(union.Byte2);
		}

		public override short Deserialize(ValueStream stream)
		{
			return new UnifiedUnion(
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte()).Int16;
		}
	}
}
