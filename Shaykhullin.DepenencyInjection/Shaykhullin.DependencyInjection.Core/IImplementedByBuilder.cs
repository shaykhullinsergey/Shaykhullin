using System;

namespace Shaykhullin.DependencyInjection.Core
{
	public interface IImplementedByBuilder<in TRegister> : ILifecycleBuilder
		where TRegister : class
	{
		ILifecycleBuilder ImplementedBy<TImplemented>(Func<IContainer, TImplemented> factory = null)
			where TImplemented : TRegister;

		ILifecycleBuilder ImplementedBy(Type implemented, Func<IContainer, object> factory = null);
	}
}