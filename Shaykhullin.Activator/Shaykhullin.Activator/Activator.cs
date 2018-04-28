using System;

namespace Shaykhullin.Activator.Core
{
	internal class Activator : IActivator
	{
		public TObject Create<TObject>(params object[] args)
		{
			if(args.Length == 0)
			{
				return System.Activator.CreateInstance<TObject>();
			}

			return (TObject)Create(typeof(TObject), args);
		}

		public object Create(Type type, params object[] args)
		{
			return args.Length == 0 
				? System.Activator.CreateInstance(type) 
				: System.Activator.CreateInstance(type, args);
		}
	}
}
