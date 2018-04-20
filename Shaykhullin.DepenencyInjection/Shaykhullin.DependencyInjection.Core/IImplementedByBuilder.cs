using System;

namespace Shaykhullin.DependencyInjection.Core
{
	public interface IImplementedByBuilder<in TRegistry> : ILifecycleBuilder
	{
		ILifecycleBuilder ImplementedBy<TImplementation>(Func<IContainer, TImplementation> factory = null)
			where TImplementation : TRegistry;

		ILifecycleBuilder ImplementedBy(Type implemented, Func<IContainer, object> factory = null);
	}
}