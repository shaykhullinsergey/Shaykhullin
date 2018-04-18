using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Shaykhullin.Serializer.Tests
{
	public class DateTimeTests : SerializerTests
	{
		[Fact]
		public void DateTimeUtcSerializing()
		{
			var serializer = new SerializerConfig().Create();

			var input = new DateTime(2018, 01, 02, 03, 04, 05, DateTimeKind.Utc);

			using (var stream = CreateStream())
			{
				serializer.Serialize(stream, input);
				stream.Position = 0;
				var result = serializer.Deserialize<DateTime>(stream);

				Assert.Equal(2018, result.Year);
				Assert.Equal(01, result.Month);
				Assert.Equal(2, result.Day);
				Assert.Equal(3, result.Hour);
				Assert.Equal(4, result.Minute);
				Assert.Equal(5, result.Second);
				Assert.Equal(DateTimeKind.Utc, result.Kind);
			}
		}

		[Fact]
		public void DateTimeLocalSerializing()
		{
			var serializer = new SerializerConfig().Create();

			var input = new DateTime(2018, 01, 02, 03, 04, 05, DateTimeKind.Local);

			using (var stream = CreateStream())
			{
				serializer.Serialize(stream, input);
				stream.Position = 0;
				var result = serializer.Deserialize<DateTime>(stream);

				Assert.Equal(2018, result.Year);
				Assert.Equal(01, result.Month);
				Assert.Equal(2, result.Day);
				Assert.Equal(3, result.Hour);
				Assert.Equal(4, result.Minute);
				Assert.Equal(5, result.Second);
				Assert.Equal(DateTimeKind.Local, result.Kind);
			}
		}

		[Fact]
		public void DateTimeUnspecifiedSerializing()
		{
			var serializer = new SerializerConfig().Create();

			var input = new DateTime(2018, 01, 02, 03, 04, 05, DateTimeKind.Unspecified);

			using (var stream = CreateStream())
			{
				serializer.Serialize(stream, input);
				stream.Position = 0;
				var result = serializer.Deserialize<DateTime>(stream);

				Assert.Equal(2018, result.Year);
				Assert.Equal(01, result.Month);
				Assert.Equal(2, result.Day);
				Assert.Equal(3, result.Hour);
				Assert.Equal(4, result.Minute);
				Assert.Equal(5, result.Second);
				Assert.Equal(DateTimeKind.Unspecified, result.Kind);
			}
		}
	}
}
