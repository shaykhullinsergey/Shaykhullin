using System.IO;

namespace Shaykhullin.Serializer.Core
{
	internal class ByteConverter : Converter<byte>
	{
		public override void Serialize(Stream stream, byte data)
		{
			stream.WriteByte(data);
		}

		public override byte Deserialize(Stream stream)
		{
			return (byte)stream.ReadByte();
		}
	}
}
