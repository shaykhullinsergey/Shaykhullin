using System.IO;
using Shaykhullin.Stream;

namespace Shaykhullin.Serializer.Core
{
	internal class BoolConverter : Converter<bool>
	{
		public override void Serialize(ValueStream stream, bool data)
		{
			stream.WriteByte((byte)(data ? 1 : 0));
		}

		public override bool Deserialize(ValueStream stream)
		{
			return stream.ReadByte() == 1;
		}
	}
}
