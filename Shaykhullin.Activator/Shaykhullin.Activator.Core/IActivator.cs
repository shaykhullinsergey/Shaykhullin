using System;

namespace Shaykhullin.Activator
{
	public interface IActivator
	{
		TObject Create<TObject, TArg1>(TArg1 arg1);
		TObject Create<TObject>(params object[] args);
		
		object Create(Type type, params object[] args);
		object Create<TArg1>(Type type, TArg1 arg1);
	}
}
