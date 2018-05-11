using System;
using System.IO;
using Shaykhullin.Stream;

namespace Shaykhullin.Serializer.Core
{
	internal class GuidConverter : Converter<Guid>
	{
		public override Guid Deserialize(ValueStream stream)
		{
			var guidBytes = new byte[16];
			stream.Read(guidBytes, 0, guidBytes.Length);
			return new Guid(guidBytes);
		}

		public override void Serialize(ValueStream stream, Guid data)
		{
			var guidBytes = data.ToByteArray();
			stream.Write(guidBytes, 0, guidBytes.Length);
		}
	}
}