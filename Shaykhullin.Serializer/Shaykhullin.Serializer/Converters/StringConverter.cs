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
			var union = new UnifiedUnion(
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte());

			var length = union.Int32;

			var stringBuffer = new byte[length];
			stream.Read(stringBuffer, 0, stringBuffer.Length);
			return Encoding.UTF8.GetString(stringBuffer);
		}

		public override void Serialize(Stream stream, string data)
		{
			var union = new UnifiedUnion(data.Length);
			stream.WriteByte(union.Byte1);
			stream.WriteByte(union.Byte2);
			stream.WriteByte(union.Byte3);
			stream.WriteByte(union.Byte4);
			
			var stringBuffer = Encoding.UTF8.GetBytes(data);
			stream.Write(stringBuffer, 0, stringBuffer.Length);
		}
	}
}
