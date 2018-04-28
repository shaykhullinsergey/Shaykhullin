using System.Dynamic;
using Xunit;

namespace Shaykhullin.Serializer.Tests
{
	public class Uint64Tests : SerializerTests
	{
		[Fact]
		public void UInt64MaxValueSerializing()
		{
			using (var config = new SerializerConfig())
			{
				using (var serializer = config.Create())
				{
					using (var stream = CreateStream())
					{
						serializer.Serialize(stream, ulong.MaxValue);
						stream.Position = 0;
						var result = serializer.Deserialize<ulong>(stream);
						Assert.Equal(ulong.MaxValue, result);
					}
				}
			}
		}
	}
}