using System;

namespace Shaykhullin.DependencyInjection.Core
{
	public struct LifecycleBuilder
	{
		private readonly Dependency dependency;

		internal LifecycleBuilder(Dependency dependency)
		{
			this.dependency = dependency ?? throw new ArgumentNullException(nameof(dependency));
		}

		public ForBuilder As<TLifecycle>()
			where TLifecycle : ILifecycle
		{
			return As(typeof(TLifecycle));
		}

		public ForBuilder As(Type lifecycle)
		{
			dependency.LifecycleType = lifecycle;
			return new ForBuilder(dependency);
		}

		public void For<TDependency>()
		{
			For(typeof(TDependency));
		}

		public void For(Type type)
		{
			new ForBuilder(dependency)
				.For(type);
		}
	}
}