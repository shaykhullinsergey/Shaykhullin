using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Shaykhullin.Serializer.Core
{
	internal class StringConverter : Converter<string>
	{
		public override string Deserialize(Stream stream)
		{
			var lengthBuffer = new byte[4];
			stream.Read(lengthBuffer, 0, lengthBuffer.Length);
			var length = BitConverter.ToInt32(lengthBuffer, 0);

			var stringBuffer = new byte[length];
			stream.Read(stringBuffer, 0, stringBuffer.Length);
			return Encoding.UTF8.GetString(stringBuffer);
		}

		public override void Serialize(Stream stream, string data)
		{
			var lengthBuffer = BitConverter.GetBytes(data.Length);
			stream.Write(lengthBuffer, 0, lengthBuffer.Length);

			var stringBuffer = Encoding.UTF8.GetBytes(data);
			stream.Write(stringBuffer, 0, stringBuffer.Length);
		}
	}
}
