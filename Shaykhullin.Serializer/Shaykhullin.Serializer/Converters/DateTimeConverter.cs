using System;
using System.IO;

namespace Shaykhullin.Serializer.Core
{
	internal class DateTimeConverter : Converter<DateTime>
	{
		public override DateTime Deserialize(Stream stream)
		{
			var dateTimeBytes = new byte[8];
			stream.Read(dateTimeBytes, 0, dateTimeBytes.Length);
			var dateTimeBinary = BitConverter.ToInt64(dateTimeBytes, 0);
			return DateTime.FromBinary(dateTimeBinary);
		}

		public override void Serialize(Stream stream, DateTime data)
		{
			var dateTimeBytes = BitConverter.GetBytes(data.ToBinary());
			stream.Write(dateTimeBytes, 0, dateTimeBytes.Length);
		}
	}
}