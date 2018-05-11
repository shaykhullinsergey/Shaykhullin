using System.IO;
using Shaykhullin.Stream;

namespace Shaykhullin.Serializer.Core
{
	internal class ByteConverter : Converter<byte>
	{
		public override void Serialize(ValueStream stream, byte data)
		{
			stream.WriteByte(data);
		}

		public override byte Deserialize(ValueStream stream)
		{
			return (byte)stream.ReadByte();
		}
	}
}
