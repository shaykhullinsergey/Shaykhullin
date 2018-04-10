using System.IO;

namespace Shaykhullin.Serializer.Core
{
	internal class BoolConverter : Converter<bool>
	{
		public override void Serialize(Stream stream, bool data)
		{
			stream.WriteByte((byte)(data ? 1 : 0));
		}

		public override bool Deserialize(Stream stream)
		{
			return stream.ReadByte() == 1 ? true : false;
		}
	}
}
