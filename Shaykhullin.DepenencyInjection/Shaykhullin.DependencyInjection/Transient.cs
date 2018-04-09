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
			this.activator = activator;
		}

		public object Resolve(Type type, object[] arguments)
		{
			return activator.Create(type, arguments);
		}

		public object Resolve(Func<object> factory)
		{
			return factory();
		}
	}
}