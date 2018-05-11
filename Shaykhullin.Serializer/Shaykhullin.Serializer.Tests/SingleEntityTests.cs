using System.Text;
using Xunit;

namespace Shaykhullin.Serializer.Tests
{
	public class SingleEntityTests : SerializerTests
	{
		public class Entity
		{
			public int Id { get; set; }
		}

		[Fact]
		public void EntitySerializing()
		{
			var serializer = new SerializerConfig().Create();

			var input = new Entity
			{
				Id = 123
			};

			using (var stream = CreateStream())
			{
				serializer.Serialize(stream, input);
				stream.Seek(0);
				var result = serializer.Deserialize<Entity>(stream);

				Assert.NotNull(result);
				Assert.Equal(123, result.Id);
			}
		}

		public class StringEntity
		{
			public string Name { get; set; }
		}

		[Fact]
		public void StringEntitySerializing()
		{
			var serializer = new SerializerConfig().Create();

			var input = new StringEntity
			{
				Name = Encoding.UTF8.GetString(new byte[] { 1, 2, 3, 4, 5 })
			};

			using (var stream = CreateStream())
			{
				serializer.Serialize(stream, input);
				stream.Seek(0);
				var result = serializer.Deserialize<StringEntity>(stream);

				Assert.NotNull(result);
				Assert.Equal(input.Name, result.Name);
			}
		}

		public class EntityHolder
		{
			public Entity Entity { get; set; }
		}

		public class DerivedEntity : Entity
		{
			public string Discriminator { get; set; }
		}

		[Fact]
		public void NotRegisteredDerivedEntitySerializedAsBaseEntity()
		{
			var serializer = new SerializerConfig().Create();

			var input = new EntityHolder
			{
				Entity = new DerivedEntity
				{
					Id = 111,
					Discriminator = "DerivedEntity"
				}
			};

			using (var stream = CreateStream())
			{
				serializer.Serialize(stream, input);
				stream.Seek(0);
				var result = serializer.Deserialize<EntityHolder>(stream);

				Assert.NotNull(result);
				Assert.NotNull(result.Entity);
				Assert.IsType<Entity>(result.Entity);
				Assert.Equal(111, result.Entity.Id);
			}
		}
	}
}
