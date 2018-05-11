using Xunit;

namespace Shaykhullin.Serializer.Tests
{
	public class NullTests : SerializerTests
	{
		class EntityForNull
		{
		}

		[Fact]
		public void NullRestores()
		{
			using (var config = new SerializerConfig())
			{
				config.Match<EntityForNull>();

				using (var serializer = config.Create())
				{
					using (var stream = CreateStream())
					{
						serializer.Serialize(stream, (EntityForNull)null);
						stream.Seek(0);
						var result = serializer.Deserialize<EntityForNull>(stream);
						Assert.Null(result);
					}
				}
			}
		}

		[Fact]
		public void NullInArrayRestores()
		{
			using (var config = new SerializerConfig())
			{
				config.Match<EntityForNull>();

				var input = new EntityForNull[]
				{
					new EntityForNull(),
					null,
					new EntityForNull()
				};

				using (var serializer = config.Create())
				{
					using (var stream = CreateStream())
					{
						serializer.Serialize(stream, input);
						stream.Seek(0);
						var result = serializer.Deserialize<EntityForNull[]>(stream);

						Assert.Equal(3, result.Length);
						Assert.NotNull(result[0]);
						Assert.Null(result[1]);
						Assert.NotNull(result[2]);
					}
				}
			}
		}

		class NullHolder
		{
			public EntityForNull Null { get; set; }
		}

		[Fact]
		public void NullHolderWithNullSerializing()
		{
			var input = new NullHolder
			{
				Null = null
			};

			using (var config = new SerializerConfig())
			{
				using (var serializer = config.Create())
				{
					using (var stream = CreateStream())
					{
						serializer.Serialize(stream, input);
						stream.Seek(0);
						var result = serializer.Deserialize<NullHolder>(stream);

						Assert.NotNull(result);
						Assert.Null(result.Null);
					}
				}
			}
		}
	}
}
