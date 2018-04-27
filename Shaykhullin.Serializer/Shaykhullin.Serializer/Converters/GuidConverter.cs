using System;
using System.IO;

namespace Shaykhullin.Serializer
{
	public class GuidConverter : Converter<Guid>
	{
		public override Guid Deserialize(Stream stream)
		{
			var guidBytes = new byte[16];
			stream.Read(guidBytes, 0, guidBytes.Length);
			return new Guid(guidBytes);
		}

		public override void Serialize(Stream stream, Guid data)
		{
			var guidBytes = data.ToByteArray();
			stream.Write(guidBytes, 0, guidBytes.Length);
		}
	}
}