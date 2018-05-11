using System.IO;
using Shaykhullin.Stream;

namespace Shaykhullin.Serializer.Core
{
	internal class SByteConverter : Converter<sbyte>
	{
		public override void Serialize(ValueStream stream, sbyte data)
		{
			stream.WriteByte((byte)data);
		}

		public override sbyte Deserialize(ValueStream stream)
		{
			return (sbyte)stream.ReadByte();
		}
	}
}
