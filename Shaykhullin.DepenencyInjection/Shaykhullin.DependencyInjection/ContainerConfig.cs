using System;

using Shaykhullin.Activator;
using Shaykhullin.DependencyInjection.Core;


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

		public IImplementedByBuilder<TRegistry> Register<TRegistry>()
		{
			return Register<TRegistry>(typeof(TRegistry));
		}

		public IImplementedByBuilder<object> Register(Type register)
		{
			return Register<object>(register);
		}

		public IImplementedByBuilder<TRegistry> Register<TRegistry>(Type register)
		{
			return new RegisterBuilder(dependencyContainer)
				.Register<TRegistry>(register);
		}

		public IContainerConfig CreateScope() => new ContainerConfig(this);

		public IContainer Create() => new Container(activator, dependencyContainer);

		public void Dispose() { }
	}
}