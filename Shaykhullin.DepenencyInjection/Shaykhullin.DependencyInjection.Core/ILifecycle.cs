using System;

namespace Shaykhullin.DependencyInjection.Core
{
	public interface ILifecycle
	{
		object Resolve(Type type, object[] arguments);
		object Resolve<TState>(Func<TState, object> factory, TState state);
	}
}