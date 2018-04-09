using System;

namespace Shaykhullin.Activator
{
	public interface IActivator
	{
		TObject Create<TObject>(params object[] args);
		object Create(Type type, params object[] args);
	}
}
