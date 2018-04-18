using System;
using Xunit;

namespace Shaykhullin.DependencyInjection.Tests
{
	public class SingletonTests
	{
		class SingletonEntity
		{
		}
		
		[Fact]
		public void SingletonWorks()
		{
			var config = new ContainerConfig();

			config.Register<SingletonEntity>()
				.As<Singleton>();

			using (var container = config.Create())
			{
				var test1 = container.Resolve<SingletonEntity>();
				var test2 = container.Resolve<SingletonEntity>();
				
				Assert.Same(test1, test2);
			}
		}

		class SingletonEntityHolder
		{
			public SingletonEntityHolder(SingletonEntity singletonEntity)
			{
				SingletonEntity = singletonEntity;
			}
			
			public SingletonEntity SingletonEntity { get; set; }
		}

		[Fact]
		public void SingletonInSingletonWorks()
		{
			var config = new ContainerConfig();

			config.Register<SingletonEntity>()
				.As<Singleton>();

			config.Register<SingletonEntityHolder>()
				.As<Singleton>();

			using (var container = config.Create())
			{
				var test1 = container.Resolve<SingletonEntityHolder>();
				var test2 = container.Resolve<SingletonEntityHolder>();
				
				Assert.Same(test1, test2);
				Assert.Same(test1.SingletonEntity, test2.SingletonEntity);
			}
		}
		
		[Fact]
		public void ImplementedByWithSingletonWorks()
		{
			var config = new ContainerConfig();

			config.Register<SingletonEntity>()
				.ImplementedBy(c => new SingletonEntity())
				.As<Singleton>();

			using (var container = config.Create())
			{
				var test1 = container.Resolve<SingletonEntity>();
				var test2 = container.Resolve<SingletonEntity>();
				
				Assert.Same(test1, test2);
			}
		}
	}
}