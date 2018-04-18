using Xunit;

namespace Shaykhullin.DependencyInjection.Tests
{
	public class TransientTests
	{
		class TransientEntity
		{
		}
		
		[Fact]
		public void TransientWorks()
		{
			var config = new ContainerConfig();

			config.Register<TransientEntity>()
				.As<Transient>();

			using (var container = config.Create())
			{
				var test1 = container.Resolve<TransientEntity>();
				var test2 = container.Resolve<TransientEntity>();
				
				Assert.NotSame(test1, test2);
			}
		}

		class TransientEntityHolder
		{
			public TransientEntityHolder(TransientEntity transientEntity)
			{
				TransientEntity = transientEntity;
			}
			
			public TransientEntity TransientEntity { get; set; }
		}

		[Fact]
		public void TransientInTransientWorks()
		{
			var config = new ContainerConfig();

			config.Register<TransientEntity>()
				.As<Transient>();

			config.Register<TransientEntityHolder>()
				.As<Transient>();

			using (var container = config.Create())
			{
				var test1 = container.Resolve<TransientEntityHolder>();
				var test2 = container.Resolve<TransientEntityHolder>();
				
				Assert.NotSame(test1, test2);
				Assert.NotSame(test1.TransientEntity, test2.TransientEntity);
			}
		}
		
		[Fact]
		public void ImplementedByWithTransientWorks()
		{
			var config = new ContainerConfig();

			config.Register<TransientEntity>()
				.ImplementedBy(c => new TransientEntity())
				.As<Transient>();

			using (var container = config.Create())
			{
				var test1 = container.Resolve<TransientEntity>();
				var test2 = container.Resolve<TransientEntity>();
				
				Assert.NotSame(test1, test2);
			}
		}
	}
}