using System;
using System.IO;

namespace Shaykhullin.Serializer.Core
{
	internal class Int32Converter : Converter<int>
	{
		public override void Serialize(Stream stream, int data)
		{
			var bytes = BitConverter.GetBytes(data);
			stream.Write(bytes, 0, bytes.Length);
		}

		public override int Deserialize(Stream stream)
		{
			var bytes = new byte[4];
			stream.Read(bytes, 0, bytes.Length);
			return BitConverter.ToInt32(bytes, 0);
		}
	}
}
