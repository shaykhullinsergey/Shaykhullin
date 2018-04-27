using System;
using Shaykhullin.Activator;
using Shaykhullin.DependencyInjection.Core;

namespace Shaykhullin.DependencyInjection
{
	public class Transient : ILifecycle
	{
		private readonly IActivator activator;

		public Transient(IActivator activator)
		{
			this.activator = activator ?? throw new ArgumentNullException(nameof(activator));
		}

		public object Resolve(Type type, object[] arguments)
		{
			return activator.Create(type, arguments);
		}

		public object Resolve<TState>(Func<TState, object> factory, TState state)
		{
			return factory(state);
		}
	}
}