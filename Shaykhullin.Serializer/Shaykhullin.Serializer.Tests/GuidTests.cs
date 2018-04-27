using Xunit;
using System;

namespace Shaykhullin.Serializer.Tests
{
	public class GuidTests : SerializerTests
	{
		[Fact]
		public void GuidSerializing()
		{
			var input = Guid.NewGuid();
			
			using (var config = new SerializerConfig())
			{
				using (var serializer = config.Create())
				{
					using (var stream = CreateStream())
					{
						serializer.Serialize(stream, input);
						stream.Position = 0;
						var result = serializer.Deserialize<Guid>(stream);
						
						Assert.Equal(input, result);
					}
				}
			}
		}
	}
}