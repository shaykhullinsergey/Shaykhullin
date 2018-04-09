using System;

namespace Shaykhullin.DependencyInjection.Core
{
	public interface ILifecycleBuilder : IForBuilder
	{
		IForBuilder As<TLifecycle>()
			where TLifecycle : ILifecycle;

		IForBuilder As(Type lifecycle);
	}
	
	public interface IForBuilder
	{
		void For<TDependency>();
		void For(Type dependency);
	}
}