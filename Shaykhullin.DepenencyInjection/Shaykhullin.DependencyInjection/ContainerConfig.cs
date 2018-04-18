using System;
using Shaykhullin.DependencyInjection.Core;

using Shaykhullin.Activator;
using Shaykhullin.Activator.Core;

namespace Shaykhullin.DependencyInjection
{
	public class ContainerConfig : IContainerConfig
	{
		private readonly IActivator activator;
		private readonly DependencyContainer dependencyContainer;

		public ContainerConfig()
		{
			activator = new ActivatorConfig().Create();
			dependencyContainer = new DependencyContainer();
		}

		internal ContainerConfig(ContainerConfig parent) 
		{
			activator = parent.activator;
			dependencyContainer = new DependencyContainer(parent.dependencyContainer);
		}

		public IImplementedByBuilder<TRegister> Register<TRegister>()
		{
			return Register<TRegister>(typeof(TRegister));
		}

		public IImplementedByBuilder<object> Register(Type register)
		{
			return Register<object>(register);
		}

		public IImplementedByBuilder<TRegister> Register<TRegister>(Type register)
		{
			return new RegisterBuilder(dependencyContainer)
				.Register<TRegister>(register);
		}

		public IContainerConfig CreateScope() => new ContainerConfig(this);

		public IContainer Create() => new Container(activator, dependencyContainer);

		public void Dispose() { }
	}
}