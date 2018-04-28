using Xunit;

namespace Shaykhullin.Serializer.Tests
{
	public class DoubleTests : SerializerTests
	{
		[Fact]
		public void DoubleSerializing()
		{
			var serializer = new SerializerConfig().Create();

			using (var stream = CreateStream())
			{
				serializer.Serialize(stream, 12.2);
				stream.Position = 0;
				var result = serializer.Deserialize<double>(stream);
				Assert.Equal(12.2, result);
			}
		}
		
		[Fact]
		public void DoubleMaxValueSerializing()
		{
			var serializer = new SerializerConfig().Create();

			using (var stream = CreateStream())
			{
				serializer.Serialize(stream, double.MaxValue);
				stream.Position = 0;
				var result = serializer.Deserialize<double>(stream);
				Assert.Equal(double.MaxValue, result);
			}
		}
	}
}
