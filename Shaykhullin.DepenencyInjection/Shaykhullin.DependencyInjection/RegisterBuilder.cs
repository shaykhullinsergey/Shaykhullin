using System;
using System.Collections.Generic;

namespace Shaykhullin.DependencyInjection.Core
{
	internal class RegisterBuilder : IRegisterBuilder
	{
		private readonly DependencyContainer dependencies;

		public RegisterBuilder(DependencyContainer dependencies)
		{
			this.dependencies = dependencies;
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
			var dto = dependencies.Register(register);
			return new ImplementedByBuilder<TRegistry>(dto);
		}
	}
}