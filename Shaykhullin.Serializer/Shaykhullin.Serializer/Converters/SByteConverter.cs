using System.IO;

namespace Shaykhullin.Serializer.Core
{
	internal class SByteConverter : Converter<sbyte>
	{
		public override void Serialize(Stream stream, sbyte data)
		{
			stream.WriteByte((byte)data);
		}

		public override sbyte Deserialize(Stream stream)
		{
			return (sbyte)stream.ReadByte();
		}
	}
}
