using System;

namespace Shaykhullin.DependencyInjection.Core
{
	internal class LifecycleBuilder : ILifecycleBuilder
	{
		private readonly Dependency dependency;
		
		public LifecycleBuilder(Dependency dependency)
		{
			this.dependency = dependency ?? throw new ArgumentNullException(nameof(dependency));
		}

		public IForBuilder As<TLifecycle>() where TLifecycle : ILifecycle => As(typeof(TLifecycle));
		public IForBuilder As(Type lifecycle)
		{
			dependency.LifecycleType = lifecycle;
			return new ForBuilder(dependency);
		}

		public void For<TDependency>() => For(typeof(TDependency));
		public void For(Type type)
		{
			new ForBuilder(dependency)
				.For(type);
		}
	}
}