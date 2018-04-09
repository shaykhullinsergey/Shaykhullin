using System;

namespace Shaykhullin.DependencyInjection.Core
{
	internal class LifecycleBuilder : ILifecycleBuilder
	{
		private readonly Dependency dto;
		
		public LifecycleBuilder(Dependency dto)
		{
			this.dto = dto;
		}

		public IForBuilder As<TLifecycle>() where TLifecycle : ILifecycle => As(typeof(TLifecycle));
		public IForBuilder As(Type lifecycle)
		{
			dto.Lifecycle = lifecycle;
			return new ForBuilder(dto);
		}

		public void For<TDependency>() => For(typeof(TDependency));
		public void For(Type dependency)
		{
			new ForBuilder(dto)
				.For(dependency);
		}
	}
}