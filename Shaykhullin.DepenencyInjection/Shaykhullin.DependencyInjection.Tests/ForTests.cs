using Xunit;

namespace Shaykhullin.DependencyInjection.Tests
{
	public class ForTests
	{
		class Entity1
		{
			public Entity1(Entity3 entity3)
			{
				Entity3 = entity3;
			}
			
			public Entity3 Entity3 { get; set; }
		}

		class Entity2
		{
			public Entity2(Entity3 entity3)
			{
				Entity3 = entity3;
			}
			
			public Entity3 Entity3 { get; set; }
		}

		class Entity3
		{
			public int Id { get; set; }
		}

		[Fact]
		public void ForWorksForDifferentEntities()
		{
			var config = new ContainerConfig();

			config.Register<Entity1>();
			config.Register<Entity2>();
			
			config.Register<Entity3>()
				.ImplementedBy(c => new Entity3{Id = 11})
				.As<Singleton>()
				.For<Entity1>();
			
			config.Register<Entity3>()
				.ImplementedBy(c => new Entity3{Id = 12})
				.As<Singleton>()
				.For<Entity2>();

			using (var container = config.Create())
			{
				var entity1 = container.Resolve<Entity1>();
				var entity2 = container.Resolve<Entity2>();
				
				Assert.Equal(11, entity1.Entity3.Id);
				Assert.Equal(12, entity2.Entity3.Id);
			}
		}

		public void Works()
		{
			using (var config = new ContainerConfig())
			{
				config.Register<Entity1>();
				config.Register<Entity2>();

				using (var container = config.Create())
				{
					var entity1 = container.Resolve<Entity1>();
					var entity2 = container.Resolve<Entity2>();
				}
			}
		}
	}
}