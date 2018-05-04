using Xunit;

namespace Shaykhullin.DependencyInjection.Tests
{
	public class PerformanceTests
	{
		public class Entity
		{
			public Entity(Inject inject)
			{
				Inject = inject;
			}
			
			public Inject Inject { get; }
		}

		public class Inject
		{
			public int Int32 { get; set; }
		}
		
		[Fact]
		public void PerformanceTest()
		{
			var config = new ContainerConfig();

			config.Register<Entity>()
				.As<Singleton>();

			config.Register<Inject>()
				.As<Singleton>();

			for (int i = 0; i < 10_000; i++)
			{
				using (var scope = config.CreateScope())
				{
					using (var container = scope.Create())
					{
						var entity = container.Resolve<Entity>();
						
						Assert.Equal(0, entity.Inject.Int32);
					}
				}
			}
		}
	}
}