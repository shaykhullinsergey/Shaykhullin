using System;

namespace Shaykhullin.DependencyInjection
{
	public interface IContainer : IDisposable
	{
		object Resolve(Type type);

		TResolve Resolve<TResolve>();
	}
}