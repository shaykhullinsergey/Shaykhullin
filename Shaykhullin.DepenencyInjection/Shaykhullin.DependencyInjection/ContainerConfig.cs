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
			where TRegister : class =>
				new RegisterBuilder(dependencyContainer)
					.Register<TRegister>();

		public IImplementedByBuilder<object> Register(Type register) =>
			new RegisterBuilder(dependencyContainer)
				.Register(register);

		public IContainerConfig Scope() => new ContainerConfig(this);

		public IContainer Container => container ?? (container = new Container(activatorConfig.Create(), dependencyContainer));

		public void Dispose() { }
	}
}