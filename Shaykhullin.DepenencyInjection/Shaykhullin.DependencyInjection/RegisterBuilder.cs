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
			where TRegister : class
		{
			return Register(typeof(TRegister));
		}

		public IImplementedByBuilder<object> Register(Type register)
		{
			var dto = dependencies.Register(register);
			return new ImplementedByBuilder<object>(dto);
		}
	}
}