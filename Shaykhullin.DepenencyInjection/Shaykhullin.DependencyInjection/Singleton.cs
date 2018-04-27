using System;
using Shaykhullin.Activator;
using Shaykhullin.DependencyInjection.Core;

namespace Shaykhullin.DependencyInjection
{
	public class Singleton : ILifecycle
	{
		private object instance;
		private readonly IActivator activator;

		public Singleton(IActivator activator)
		{
			this.activator = activator ?? throw new ArgumentNullException(nameof(activator));
		}

		public object Resolve(Type type, object[] arguments)
		{
			return instance ?? (instance = activator.Create(type, arguments));
		}

		public object Resolve<TState>(Func<TState, object> factory, TState state)
		{
			return instance ?? (instance = factory(state));
		}
	}
}