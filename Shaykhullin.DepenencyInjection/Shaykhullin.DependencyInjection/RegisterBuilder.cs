using System;
using System.Collections.Generic;

namespace Shaykhullin.DependencyInjection.Core
{
	internal class RegisterBuilder : IRegisterBuilder
	{
		private readonly DependencyContainer dependencies;

		public RegisterBuilder(DependencyContainer dependencies)
		{
			this.dependencies = dependencies ?? throw new ArgumentNullException(nameof(dependencies));
		}
		
		public IImplementedByBuilder<TRegistry> Register<TRegistry>() 
		{
			return Register<TRegistry>(typeof(TRegistry));
		}

		public IImplementedByBuilder<object> Register(Type registry)
		{
			return Register<object>(registry);
		}

		public IImplementedByBuilder<TRegistry> Register<TRegistry>(Type registry)
		{
			var dto = dependencies.Register(registry);
			return new ImplementedByBuilder<TRegistry>(dto);
		}
	}
}