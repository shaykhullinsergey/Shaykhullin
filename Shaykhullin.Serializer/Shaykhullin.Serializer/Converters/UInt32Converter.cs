using System;
using System.IO;

namespace Shaykhullin.Serializer.Core
{
	internal class UInt32Converter : Converter<uint>
	{
		public override void Serialize(Stream stream, uint data)
		{
			var bytes = BitConverter.GetBytes(data);
			stream.Write(bytes, 0, bytes.Length);
		}

		public override uint Deserialize(Stream stream)
		{
			var bytes = new byte[4];
			stream.Read(bytes, 0, bytes.Length);
			return BitConverter.ToUInt32(bytes, 0);
		}
	}
}
