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

		class Person
		{
			public string Name { get; set; }
		}

		[Fact]
		public void StringNameInPersonIsSerializing()
		{
			var serializer = new SerializerConfig().Create();

			var input = new Person
			{
				Name = "WORKS"
			};

			using (var stream = CreateStream())
			{
				serializer.Serialize(stream, input);
				stream.Position = 0;
				var result = serializer.Deserialize<Person>(stream);

				Assert.Equal("WORKS", result.Name);
			}
		}

		[Fact]
		public void StringNameInPersonIsSerializingInScope()
		{
			var config = new SerializerConfig();

			var input = new Person
			{
				Name = "WORKS"
			};

			using (var scope = config.CreateScope())
			{
				scope.Match<string>();
				
				var serializer = scope.Create();
				
				
				using (var stream = CreateStream())
				{
					serializer.Serialize(stream, input);
					stream.Position = 0;
					var result = serializer.Deserialize<Person>(stream);

					Assert.Equal("WORKS", result.Name);
				}
			}
		}
	}
}
