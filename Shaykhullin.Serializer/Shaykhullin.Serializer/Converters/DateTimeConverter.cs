using System;
using System.IO;
using Shaykhullin.Stream;

namespace Shaykhullin.Serializer.Core
{
	internal class DateTimeConverter : Converter<DateTime>
	{
		public override void Serialize(ValueStream stream, DateTime data)
		{
			var binary = data.ToBinary();
			
			var union = new UnifiedUnion(binary);
			stream.WriteByte(union.Byte1);
			stream.WriteByte(union.Byte2);
			stream.WriteByte(union.Byte3);
			stream.WriteByte(union.Byte4);
			stream.WriteByte(union.Byte5);
			stream.WriteByte(union.Byte6);
			stream.WriteByte(union.Byte7);
			stream.WriteByte(union.Byte8);
		}
		
		public override DateTime Deserialize(ValueStream stream)
		{
			var binary = new UnifiedUnion(
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte()).Int64;

			return DateTime.FromBinary(binary);
		}
	}
}