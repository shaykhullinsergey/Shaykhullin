using System;

namespace Shaykhullin.DependencyInjection.Core
{
	internal class ImplementedByBuilder<TRegistry> : IImplementedByBuilder<TRegistry>
	{
		private readonly Dependency dto;
		
		public ImplementedByBuilder(Dependency dto)
		{
			this.dto = dto;
		}

		public ILifecycleBuilder ImplementedBy<TImplemented>(Func<IContainer, TImplemented> factory = null) 
			where TImplemented : TRegistry
		{
			return ImplementedBy(typeof(TImplemented), 
				factory == null 
					? default(Func<IContainer, object>)
					: c => factory(c));
		}

		public ILifecycleBuilder ImplementedBy(Type implemented, Func<IContainer, object> factory = null)
		{
			if (factory != null)
			{
				dto.Factory = container => factory(container);
			}

			dto.Implementation = implemented;
			
			return new LifecycleBuilder(dto);
		}

		public void For<TDependency>()
		{
			For(typeof(TDependency));
		}

		public void For(Type dependency)
		{
			new LifecycleBuilder(dto)
				.For(dependency);
		}

		public IForBuilder As<TLifecycle>() where TLifecycle : ILifecycle
		{
			return As(typeof(TLifecycle));
		}

		public IForBuilder As(Type lifecycle)
		{
			return new LifecycleBuilder(dto)
				.As(lifecycle);
		}
	}
}