using Xunit;

namespace Shaykhullin.DependencyInjection.Tests
{
	public class ImplementedByTests
	{
		class Entity
		{
			public int Id { get; set; }
		}
		
		[Fact]
		public void ImplementedByWorks()
		{
			var config = new ContainerConfig();

			config.Register<Entity>()
				.ImplementedBy(c => new Entity
				{
					Id = 12
				})
				.As<Singleton>();

			using (var container = config.Create())
			{
				var result = container.Resolve<Entity>();
				Assert.Equal(12, result.Id);
			}
		}

		class DerivedEntity : Entity
		{
		}

		[Fact]
		public void ImplementedByReturnsDerivedInstance()
		{
			var config = new ContainerConfig();

			config.Register<Entity>()
				.ImplementedBy<DerivedEntity>();

			using (var container = config.Create())
			{
				var entity = container.Resolve<Entity>();
	
				Assert.IsType<DerivedEntity>(entity);
			}
		}
	}
}