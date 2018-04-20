using System;

namespace Shaykhullin.DependencyInjection.Core
{
	public interface IRegisterBuilder
	{
		IImplementedByBuilder<TRegistry> Register<TRegistry>();

		IImplementedByBuilder<TRegistry> Register<TRegistry>(Type registry);
		IImplementedByBuilder<object> Register(Type registry);
	}
}