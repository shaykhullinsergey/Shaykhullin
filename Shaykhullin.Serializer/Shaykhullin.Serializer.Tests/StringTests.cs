using Xunit;

namespace Shaykhullin.Serializer.Tests
{
	public class StringTests : SerializerTests
	{
		[Fact]
		public void StringSerializing()
		{
			var serializer = new SerializerConfig().Create();

			using (var stream = CreateStream())
			{
				var input = "String data type message";
				serializer.Serialize(stream, input);
				stream.Position = 0;
				var result = serializer.Deserialize<string>(stream);

				Assert.Equal(input, result);
			}
		}
	}
}
