using Xunit;

namespace Shaykhullin.DependencyInjection.Tests
{
	public class SingletonTransientTests
	{
		class EntityHolder
		{
			public EntityHolder(Entity entity)
			{
				Entity = entity;
			}
			
			public Entity Entity { get; set; }
		}

		class Entity
		{
		}

		[Fact]
		public void TransientEntityInSingletonHolderWillNotCreateNewTransient()
		{
			var config = new ContainerConfig();

			config.Register<EntityHolder>()
				.As<Singleton>();

			config.Register<Entity>()
				.As<Transient>();

			using (var container = config.Create())
			{
				var test1 = container.Resolve<EntityHolder>();
				var test2 = container.Resolve<EntityHolder>();
				
				Assert.Same(test1, test2);
				Assert.Same(test1.Entity, test2.Entity);
			}
		}
		
		[Fact]
		public void SingletonEntityInTransientHolderWillCreateNewTransientButSameSingleton()
		{
			var config = new ContainerConfig();

			config.Register<EntityHolder>()
				.As<Transient>();

			config.Register<Entity>()
				.As<Singleton>();

			using (var container = config.Create())
			{
				var test1 = container.Resolve<EntityHolder>();
				var test2 = container.Resolve<EntityHolder>();
				
				Assert.NotSame(test1, test2);
				Assert.Same(test1.Entity, test2.Entity);
			}
		}
	}
}