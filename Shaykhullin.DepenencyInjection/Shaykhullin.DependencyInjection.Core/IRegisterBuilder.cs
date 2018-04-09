using System;

namespace Shaykhullin.DependencyInjection.Core
{
	public interface IRegisterBuilder
	{
		IImplementedByBuilder<TRegister> Register<TRegister>();

		IImplementedByBuilder<TRegister> Register<TRegister>(Type register);
		IImplementedByBuilder<object> Register(Type register);
	}
}