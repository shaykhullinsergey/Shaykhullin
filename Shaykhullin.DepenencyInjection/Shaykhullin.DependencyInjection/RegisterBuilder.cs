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
			var dto = dependencies.Register(register);
			return new ImplementedByBuilder<TRegister>(dto);
		}
	}
}