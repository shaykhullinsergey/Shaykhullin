using System;
using System.IO;

namespace Shaykhullin.Serializer.Core
{
	internal class ShortConverter : Converter<short>
	{
		public override void Serialize(Stream stream, short data)
		{
			var bytes = BitConverter.GetBytes(data);
			stream.Write(bytes, 0, bytes.Length);
		}

		public override short Deserialize(Stream stream)
		{
			var bytes = new byte[2];
			stream.Read(bytes, 0, bytes.Length);
			return BitConverter.ToInt16(bytes, 0);
		}
	}
}
