using System;
using Shaykhullin.DependencyInjection.Core;

namespace Shaykhullin.DependencyInjection
{
	public interface IContainerConfig : IRegisterBuilder, IDisposable
	{
		IContainerConfig CreateScope();
		IContainer Create();
	}
}