using System;
using System.IO;

namespace Shaykhullin.Serializer.Core
{
	internal class SingleConverter : Converter<float>
	{
		public override void Serialize(Stream stream, float data)
		{
			var bytes = BitConverter.GetBytes(data);
			stream.Write(bytes, 0, bytes.Length);
		}

		public override float Deserialize(Stream stream)
		{
			var bytes = new byte[4];
			stream.Read(bytes, 0, bytes.Length);
			return BitConverter.ToSingle(bytes, 0);
		}
	}
}
