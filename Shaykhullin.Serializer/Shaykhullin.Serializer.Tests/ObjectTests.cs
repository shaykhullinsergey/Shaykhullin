using Xunit;

namespace Shaykhullin.Serializer.Tests
{
	public class ObjectTests : SerializerTests
	{
		[Fact]
		public void ObjectSerializing()
		{
			var serializer = new SerializerConfig().Create();

			using (var stream = CreateStream())
			{
				var input = new object();

				serializer.Serialize(stream, input);
				stream.Position = 0;
				var result = serializer.Deserialize<object>(stream);

				Assert.NotNull(result);
			}
		}
	}
}
