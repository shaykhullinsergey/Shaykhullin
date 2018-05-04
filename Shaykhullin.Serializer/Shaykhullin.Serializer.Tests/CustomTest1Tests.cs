using Xunit;
using System.Collections.Generic;

namespace Shaykhullin.Serializer.Tests
{
	public class CustomTest1Tests : SerializerTests
	{
		public class CustomTestHolder
		{
			public CustomTestBaseEntity BaseEntity { get; set; }
			public List<ListBaseNotRegisteredEntity> ListBaseEntity { get; set; }
			public ArrayBaseRegisteredEntity[] ArrayBaseEntity { get; set; }
		}

		public class CustomTestBaseEntity
		{
			public int Id { get; set; }
		}

		public class CustomTestDerivedRegisteredEntity : CustomTestBaseEntity
		{
			public string Name { get; set; }
		}

		public class ListBaseNotRegisteredEntity
		{
			public string Name { get; set; }
		}

		public class ListDerivedRegisteredEntity : ListBaseNotRegisteredEntity
		{
			public string Email { get; set; }
		}

		public class ArrayBaseRegisteredEntity
		{
			public int Id { get; set; }
		}

		public class ArrayDerivedNotRegisteredEntity : ArrayBaseRegisteredEntity
		{
			public int Post { get; set; }
		}

		[Fact]
		public void CustomTest()
		{
			var config = new SerializerConfig();
			config.Match<CustomTestDerivedRegisteredEntity>();
			config.Match<ListDerivedRegisteredEntity>();
			config.Match<ArrayBaseRegisteredEntity>();

			var serializer = config.Create();

			var input = new CustomTestHolder
			{
				BaseEntity = new CustomTestDerivedRegisteredEntity
				{
					Id = 11,
					Name = "22"
				},
				ListBaseEntity = new List<ListBaseNotRegisteredEntity>
				{
					new ListBaseNotRegisteredEntity
					{
						Name = "List1"
					},
					new ListDerivedRegisteredEntity
					{
						Name = "Derived",
						Email = "derived@derived.com"
					},
					new ListBaseNotRegisteredEntity
					{
						Name = "List2"
					}
				},
				ArrayBaseEntity = new ArrayBaseRegisteredEntity[]
				{
					new ArrayDerivedNotRegisteredEntity
					{
						Id = 11,
						Post = 22
					},
					new ArrayDerivedNotRegisteredEntity
					{
						Id = 33,
						Post = 44
					},
					new ArrayBaseRegisteredEntity
					{
						Id = 55
					}
				}
			};

			using (var stream = CreateStream())
			{
				serializer.Serialize(stream, input);
				stream.Position = 0;
				var result = serializer.Deserialize<CustomTestHolder>(stream);

				Assert.NotNull(result);

				Assert.IsType<CustomTestDerivedRegisteredEntity>(result.BaseEntity);
				var derivedEntity = result.BaseEntity as CustomTestDerivedRegisteredEntity;
				Assert.Equal(11, derivedEntity.Id);
				Assert.Equal("22", derivedEntity.Name);

				Assert.Equal(3, result.ListBaseEntity.Count);
				var list1 = result.ListBaseEntity[0];
				Assert.IsType<ListBaseNotRegisteredEntity>(list1);
				Assert.Equal("List1", list1.Name);

				var list2 = result.ListBaseEntity[1] as ListDerivedRegisteredEntity;
				Assert.NotNull(list2);
				Assert.Equal("Derived", list2.Name);
				Assert.Equal("derived@derived.com", list2.Email);

				var list3 = result.ListBaseEntity[2];
				Assert.IsType<ListBaseNotRegisteredEntity>(list3);
				Assert.Equal("List2", list3.Name);

				Assert.Equal(3, result.ArrayBaseEntity.Length);

				var element1 = result.ArrayBaseEntity[0];
				Assert.IsType<ArrayBaseRegisteredEntity>(element1);
				Assert.Equal(11, element1.Id);

				var element2 = result.ArrayBaseEntity[1];
				Assert.IsType<ArrayBaseRegisteredEntity>(element2);
				Assert.Equal(33, element2.Id);

				var element3 = result.ArrayBaseEntity[2];
				Assert.IsType<ArrayBaseRegisteredEntity>(element3);
				Assert.Equal(55, element3.Id);
			}
		}
	}
}
