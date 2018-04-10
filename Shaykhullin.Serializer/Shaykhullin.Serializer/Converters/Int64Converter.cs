using System;
using System.IO;

namespace Shaykhullin.Serializer.Core
{
	internal class Int64Converter : Converter<long>
	{
		public override void Serialize(Stream stream, long data)
		{
			var bytes = BitConverter.GetBytes(data);
			stream.Write(bytes, 0, bytes.Length);
		}

		public override long Deserialize(Stream stream)
		{
			var bytes = new byte[8];
			stream.Read(bytes, 0, bytes.Length);
			return BitConverter.ToInt64(bytes, 0);
		}
	}
}
