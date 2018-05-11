using System;

namespace Shaykhullin.DependencyInjection.Core
{
	public struct _RegisterBuilder
	{
		private readonly DependencyContainer dependencies;

		internal _RegisterBuilder(DependencyContainer dependencies)
		{
			this.dependencies = dependencies ?? throw new ArgumentNullException(nameof(dependencies));
		}
		
		public ImplementedByBuilder<TRegistry> Register<TRegistry>() 
		{
			return Register<TRegistry>(typeof(TRegistry));
		}

		public ImplementedByBuilder<object> Register(Type registry)
		{
			return Register<object>(registry);
		}

		public ImplementedByBuilder<TRegistry> Register<TRegistry>(Type registry)
		{
			var dto = dependencies.Register(registry);
			return new ImplementedByBuilder<TRegistry>(dto);
		}
	}
}