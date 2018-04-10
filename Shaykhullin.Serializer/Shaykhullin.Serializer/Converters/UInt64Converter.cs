using System;
using System.IO;

namespace Shaykhullin.Serializer.Core
{
	internal class UInt64Converter : Converter<ulong>
	{
		public override void Serialize(Stream stream, ulong data)
		{
			var bytes = BitConverter.GetBytes(data);
			stream.Write(bytes, 0, bytes.Length);
		}

		public override ulong Deserialize(Stream stream)
		{
			var bytes = new byte[8];
			stream.Read(bytes, 0, bytes.Length);
			return BitConverter.ToUInt64(bytes, 0);
		}
	}
}
