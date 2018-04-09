using System;
using Shaykhullin.DependencyInjection.Core;

using Shaykhullin.Activator;
using Shaykhullin.Activator.Core;

namespace Shaykhullin.DependencyInjection
{
	public class ContainerConfig : IContainerConfig
	{
		private IContainer container;
		private readonly IActivatorConfig activatorConfig;
		private readonly DependencyContainer dependencyContainer;

		public ContainerConfig()
		{
			activatorConfig = new ActivatorConfig();
			dependencyContainer = new DependencyContainer(null);
		}

		internal ContainerConfig(ContainerConfig parent) 
		{
			activatorConfig = parent.activatorConfig;
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

		public IContainerConfig Scope() => new ContainerConfig(this);

		public IContainer Container => container ?? (container = new Container(activatorConfig.Create(), dependencyContainer));

		public void Dispose() { }

	
	}
}