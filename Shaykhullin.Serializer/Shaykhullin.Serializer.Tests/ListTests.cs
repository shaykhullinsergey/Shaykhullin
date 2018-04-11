using Xunit;
using System.Collections.Generic;

namespace Shaykhullin.Serializer.Tests
{
	public class ListTests : SerializerTests
	{
		[Fact]
		public void ListSerializing()
		{
			var serializer = new SerializerConfig().Create();

			List<int> input = new List<int> { 1, 2, 3 };

			using (var stream = CreateStream())
			{
				serializer.Serialize(stream, input);
				stream.Position = 0;
				var result = serializer.Deserialize<List<int>>(stream);

				Assert.Equal(3, result.Count);
				Assert.Equal(input, result);
			}
		}

		[Fact]
		public void StringListSerializing()
		{
			var serializer = new SerializerConfig().Create();

			using (var stream = CreateStream())
			{
				IList<string> input = new List<string> { "ABC", "DEG", "HIJ" };
				serializer.Serialize(stream, input);
				stream.Position = 0;
				var result = serializer.Deserialize<IList<string>>(stream);

				Assert.Equal(3, result.Count);
				Assert.Equal(input, result);
			}
		}


		public class SimplePerson
		{
			public string Name { get; set; }
			public int Age { get; set; }
		}

		[Fact]
		public void SimplePersonListSerializing()
		{
			var config = new SerializerConfig();
			config.Match<SimplePerson>();

			var serializer = config.Create();

			using (var stream = CreateStream())
			{
				IList<SimplePerson> input = new List<SimplePerson>
				{
					new SimplePerson
					{
						Name = "Person1Name",
						Age = 12
					},
					new SimplePerson
					{
						Name = "Person2Name",
						Age = 24
					}
				};

				serializer.Serialize(stream, input);
				stream.Position = 0;
				var result = serializer.Deserialize<IList<SimplePerson>>(stream);

				Assert.Equal(2, result.Count);

				for (int i = 0; i < input.Count; i++)
				{
					Assert.Equal(input[i].Name, result[i].Name);
					Assert.Equal(input[i].Age, result[i].Age);
				}
			}
		}


		public class ComplexPerson
		{
			public string Name { get; set; }
			public int Age { get; set; }
			public List<ComplexPerson> Children { get; set; }
		}

		[Fact]
		public void ComplexPersonSerializing()
		{
			var config = new SerializerConfig();
			config.Match<ComplexPerson>();

			var serializer = config.Create();

			IList<ComplexPerson> input = new List<ComplexPerson>
			{
				new ComplexPerson
				{
					Name = "Sarah",
					Age = 37,
					Children = new List<ComplexPerson>
					{
						new ComplexPerson
						{
							Name = "John",
							Age = 14,
							Children = new List<ComplexPerson>()
						}
					}
				},
				new ComplexPerson
				{
					Name = "Julia",
					Age = 42,
					Children = new List<ComplexPerson>
					{
						new ComplexPerson
						{
							Name = "Sandra",
							Age = 20,
							Children = null
						}
					}
				},
				new ComplexPerson
				{
					Name = "Sam",
					Age = 14,
					Children = null
				}
			};

			using (var stream = CreateStream())
			{
				serializer.Serialize(stream, input);
				stream.Position = 0;
				var result = serializer.Deserialize<IList<ComplexPerson>>(stream);

				Assert.Equal(3, result.Count);

				var sarah = result[0];
				Assert.Equal("Sarah", sarah.Name);
				Assert.Equal(37, sarah.Age);
				Assert.Single(sarah.Children);

				var john = sarah.Children[0];
				Assert.Equal("John", john.Name);
				Assert.Equal(14, john.Age);
				Assert.Empty(john.Children);

				var julia = result[1];
				Assert.Equal("Julia", julia.Name);
				Assert.Equal(42, julia.Age);
				Assert.Single(julia.Children);

				var sandra = julia.Children[0];
				Assert.Equal("Sandra", sandra.Name);
				Assert.Equal(20, sandra.Age);
				Assert.Null(sandra.Children);

				var sam = result[2];
				Assert.Equal("Sam", sam.Name);
				Assert.Equal(14, sam.Age);
				Assert.Null(sam.Children);
			}
		}


		public class BasePerson
		{
			public string Name { get; set; }
			public int Age { get; set; }
		}

		public class ExtendedPerson : BasePerson
		{
			public string Address { get; set; }
			public int House { get; set; }
			public int Floor { get; set; }
		}

		[Fact]
		public void ExtendedPersonAsBasePersonTypeListSerializing()
		{
			var config = new SerializerConfig();

			config.Match<BasePerson>();
			config.Match<ExtendedPerson>();

			var serializer = config.Create();

			IList<BasePerson> input = new List<BasePerson>
			{
				new BasePerson
				{
					Name = "Simon",
					Age = 27
				},
				new ExtendedPerson
				{
					Name = "Maria",
					Age = 22,
					Address = "City, Street",
					House = 5,
					Floor = 3
				},
				new BasePerson
				{
					Name = "Michael",
					Age = 44
				}
			};

			using (var stream = CreateStream())
			{
				serializer.Serialize(stream, input);
				stream.Position = 0;
				var result = serializer.Deserialize<IList<BasePerson>>(stream);

				Assert.Equal(3, result.Count);

				var simon = result[0];
				Assert.IsType<BasePerson>(simon);
				Assert.Equal("Simon", simon.Name);
				Assert.Equal(27, simon.Age);

				var maria = result[1] as ExtendedPerson;
				Assert.NotNull(maria);
				Assert.Equal("Maria", maria.Name);
				Assert.Equal(22, maria.Age);
				Assert.Equal("City, Street", maria.Address);
				Assert.Equal(5, maria.House);
				Assert.Equal(3, maria.Floor);

				var michael = result[2];
				Assert.IsType<BasePerson>(michael);
				Assert.Equal("Michael", michael.Name);
				Assert.Equal(44, michael.Age);
			}
		}
	}
}
