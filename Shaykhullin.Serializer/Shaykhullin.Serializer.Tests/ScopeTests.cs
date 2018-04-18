using Xunit;

namespace Shaykhullin.Serializer.Tests
{
	public class ScopeTests : SerializerTests
	{
		class Base
		{
			public int Id { get; set; }
		}

		class Derived : Base
		{
			public int Age { get; set; }
		}
		
		[Fact]
		public void ScopeTestss()
		{
			Base b = new Derived
			{
				Id = 10,
				Age = 12
			};
			
			
			using (var config = new SerializerConfig())
			{
				using (var stream = CreateStream())
				{
					using (var serializer = config.Create())
					{
						serializer.Serialize(stream, b);
						stream.Position = 0;
						var result = serializer.Deserialize<Base>(stream);
						
						Assert.IsType<Base>(result);
					}
				}
				
				using (var scope = config.CreateScope())
				{
					scope.Match<Derived>();
					
					using (var stream = CreateStream())
					{
						using (var serializer = scope.Create())
						{
							serializer.Serialize(stream, b);
							stream.Position = 0;
							var result = serializer.Deserialize<Base>(stream);
						
							Assert.IsType<Derived>(result);
						}
					}
				}
				
				using (var stream = CreateStream())
				{
					using (var serializer = config.Create())
					{
						serializer.Serialize(stream, b);
						stream.Position = 0;
						var result = serializer.Deserialize<Base>(stream);
						
						Assert.IsType<Base>(result);
					}
				}
			}
		}
	}
}