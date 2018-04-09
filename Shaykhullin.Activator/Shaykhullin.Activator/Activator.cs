using System;

namespace Shaykhullin.Activator.Core
{
	internal class Activator : IActivator
	{
		public TObject Create<TObject>(params object[] args)
		{
			return (TObject)Create(typeof(TObject), args);
		}

		public object Create(Type type, params object[] args)
		{
			return System.Activator.CreateInstance(type, args);
		}
	}
}
