using System;
using System.IO;

namespace Shaykhullin.Serializer.Core
{
	internal class UShortConverter : Converter<ushort>
	{
		public override void Serialize(Stream stream, ushort data)
		{
			var bytes = BitConverter.GetBytes(data);
			stream.Write(bytes, 0, bytes.Length);
		}

		public override ushort Deserialize(Stream stream)
		{
			var bytes = new byte[2];
			stream.Read(bytes, 0, bytes.Length);
			return BitConverter.ToUInt16(bytes, 0);
		}
	}
}
