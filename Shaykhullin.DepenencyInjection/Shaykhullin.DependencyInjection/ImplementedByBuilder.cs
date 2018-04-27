using System;

namespace Shaykhullin.DependencyInjection.Core
{
	internal class ImplementedByBuilder<TRegistry> : IImplementedByBuilder<TRegistry>
	{
		private readonly Dependency dependency;
		
		public ImplementedByBuilder(Dependency dependency)
		{
			this.dependency = dependency;
		}

		public ILifecycleBuilder ImplementedBy<TImplemented>(Func<IContainer, TImplemented> factory = null) 
			where TImplemented : TRegistry
		{
			return factory == null 
				? ImplementedBy(typeof(TImplemented)) 
				: ImplementedBy(typeof(TImplemented), c => factory(c));
		}

		public ILifecycleBuilder ImplementedBy(Type implemented, Func<IContainer, object> factory = null)
		{
			if (factory != null)
			{
				dependency.Factory = factory;
			}

			dependency.Implementation = implemented;
			
			return new LifecycleBuilder(dependency);
		}

		public void For<TDependency>()
		{
			For(typeof(TDependency));
		}

		public void For(Type type)
		{
			new LifecycleBuilder(dependency)
				.For(type);
		}

		public IForBuilder As<TLifecycle>() where TLifecycle : ILifecycle
		{
			return As(typeof(TLifecycle));
		}

		public IForBuilder As(Type lifecycle)
		{
			return new LifecycleBuilder(dependency)
				.As(lifecycle);
		}
	}
}