using Xunit;

namespace Shaykhullin.Serializer.Tests
{
	public class FloatTests : SerializerTests
	{
		[Fact]
		public void FloatSerializing()
		{
			var serializer = new SerializerConfig().Create();

			using (var stream = CreateStream())
			{
				serializer.Serialize(stream, 12.2f);
				stream.Position = 0;
				var result = serializer.Deserialize<float>(stream);
				Assert.Equal(12.2f, result);
			}
		}
	}
}
