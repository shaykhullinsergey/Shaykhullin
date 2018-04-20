using System;

namespace Shaykhullin.DependencyInjection.Core
{
	public interface IRegisterBuilder
	{
		IImplementedByBuilder<TRegistry> Register<TRegistry>();

		IImplementedByBuilder<TRegistry> Register<TRegistry>(Type register);
		IImplementedByBuilder<object> Register(Type register);
	}
}