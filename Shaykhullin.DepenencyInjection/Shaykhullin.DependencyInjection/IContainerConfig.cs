using System;
using Shaykhullin.DependencyInjection.Core;

namespace Shaykhullin.DependencyInjection
{
	public interface IContainerConfig : IDisposable
	{
		ImplementedByBuilder<TRegistry> Register<TRegistry>();
		ImplementedByBuilder<TRegistry> Register<TRegistry>(Type registry);
		ImplementedByBuilder<object> Register(Type registry);
		
		IContainerConfig CreateScope();
		IContainer Create();
	}
}