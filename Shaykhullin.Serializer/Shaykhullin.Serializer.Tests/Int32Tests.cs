using Xunit;

namespace Shaykhullin.Serializer.Tests
{
	public class Int32Tests : SerializerTests
	{
		[Fact]
		public void Int32Serializing()
		{
			var serializer = new SerializerConfig().Create();

			using (var stream = CreateStream())
			{
				serializer.Serialize(stream, 100_000);
				stream.Seek(0);
				var result = serializer.Deserialize<int>(stream);

				Assert.Equal(100_000, result);
			}
		}
	}
}
