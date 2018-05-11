using System;

namespace Shaykhullin.DependencyInjection.Core
{
	public struct ImplementedByBuilder<TRegistry>
	{
		private readonly Dependency dependency;
		
		internal ImplementedByBuilder(Dependency dependency)
		{
			this.dependency = dependency;
		}

		public LifecycleBuilder ImplementedBy<TImplementation>(Func<IContainer, TImplementation> factory = null) 
			where TImplementation : TRegistry
		{
			return factory == null 
				? ImplementedBy(typeof(TImplementation), null) 
				: ImplementedBy(typeof(TImplementation), c => factory(c));
		}

		public LifecycleBuilder ImplementedBy(Type implemented, Func<IContainer, object> factory = null)
		{
			if (factory != null)
			{
				dependency.Factory = factory;
			}

			dependency.Implementation = implemented;
			
			return new LifecycleBuilder(dependency);
		}

		public LifecycleBuilder ImplementedBy<TImplementation>(TImplementation implementation)
			where TImplementation : TRegistry
		{
			dependency.Implementation = typeof(TImplementation);
			dependency.Instance = implementation;
			
			return new LifecycleBuilder(dependency);
		}

		public LifecycleBuilder ImplementedBy(Type type, object implementation)
		{
			dependency.Implementation = type;
			dependency.Instance = implementation;
			
			return new LifecycleBuilder(dependency);
		}

		public void For<TDependency>()
		{
			For(typeof(TDependency));
		}

		public void For(Type type)
		{
			new LifecycleBuilder(dependency).For(type);
		}

		public ForBuilder As<TLifecycle>() where TLifecycle : ILifecycle
		{
			return As(typeof(TLifecycle));
		}

		public ForBuilder As(Type lifecycle)
		{
			return new LifecycleBuilder(dependency).As(lifecycle);
		}
	}
}