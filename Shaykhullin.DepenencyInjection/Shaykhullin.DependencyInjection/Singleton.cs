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
			this.activator = activator;
		}

		public object Resolve(Type type, object[] arguments)
		{
			return instance ?? (instance = activator.Create(type, arguments));
		}

		public object Resolve(Func<object> factory)
		{
			return instance ?? (instance = factory());
		}
	}
}