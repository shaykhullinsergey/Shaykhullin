using System.IO;
using Shaykhullin.Stream;

namespace Shaykhullin.Serializer.Core
{
	internal class Int32Converter : Converter<int>
	{
		public override void Serialize(ValueStream stream, int data)
		{
			var union = new UnifiedUnion(data);
			stream.WriteByte(union.Byte1);
			stream.WriteByte(union.Byte2);
			stream.WriteByte(union.Byte3);
			stream.WriteByte(union.Byte4);
		}

		public override int Deserialize(ValueStream stream)
		{
			return new UnifiedUnion(
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte()).Int32;
		}
	}
}
