using System;

using Shaykhullin.Activator;
using Shaykhullin.ArrayPool;
using Shaykhullin.DependencyInjection.Core;

namespace Shaykhullin.DependencyInjection
{
	public class ContainerConfig : IContainerConfig
	{
		private bool disposed;
		private readonly IActivator activator;
		private readonly IArrayPool arrayPool;
		private readonly DependencyContainer dependencyContainer;
		
		public ContainerConfig()
		{
			activator = new ActivatorConfig().Create();
			arrayPool = new ArrayPoolConfig().Create();
			dependencyContainer = new DependencyContainer();
		}

		internal ContainerConfig(ContainerConfig parent) 
		{
			activator = parent.activator;
			arrayPool = parent.arrayPool;
			dependencyContainer = new DependencyContainer(parent.dependencyContainer);
		}

		public ImplementedByBuilder<TRegistry> Register<TRegistry>()
		{
			if (disposed)
			{
				throw new ObjectDisposedException(nameof(ContainerConfig));
			}
			
			return Register<TRegistry>(typeof(TRegistry));
		}

		public ImplementedByBuilder<object> Register(Type registry)
		{
			if (disposed)
			{
				throw new ObjectDisposedException(nameof(ContainerConfig));
			}

			if (registry == null)
			{
				throw new ArgumentNullException(nameof(registry));
			}
			
			return Register<object>(registry);
		}

		public ImplementedByBuilder<TRegistry> Register<TRegistry>(Type registry)
		{
			if (disposed)
			{
				throw new ObjectDisposedException(nameof(ContainerConfig));
			}

			if (registry == null)
			{
				throw new ArgumentNullException(nameof(registry));
			}
			
			return new _RegisterBuilder(dependencyContainer)
				.Register<TRegistry>(registry);
		}

		public IContainerConfig CreateScope()
		{
			if (disposed)
			{
				throw new ObjectDisposedException(nameof(ContainerConfig));
			}
			
			return new ContainerConfig(this);
		}

		public IContainer Create()
		{
			if (disposed)
			{
				throw new ObjectDisposedException(nameof(ContainerConfig));
			}
			
			return new Container(activator, dependencyContainer, arrayPool);
		}

		public void Dispose()
		{
			if (!disposed)
			{
				disposed = true;
			}
		}
	}
}