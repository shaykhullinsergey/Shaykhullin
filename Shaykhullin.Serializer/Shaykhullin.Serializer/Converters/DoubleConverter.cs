using System;
using System.IO;

namespace Shaykhullin.Serializer.Core
{
	internal class DoubleConverter : Converter<double>
	{
		public override void Serialize(Stream stream, double data)
		{
			var bytes = BitConverter.GetBytes(data);
			stream.Write(bytes, 0, bytes.Length);
		}

		public override double Deserialize(Stream stream)
		{
			var bytes = new byte[8];
			stream.Read(bytes, 0, bytes.Length);
			return BitConverter.ToDouble(bytes, 0);
		}
	}
}
