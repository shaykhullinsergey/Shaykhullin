using System;
using Shaykhullin.ArrayPool;

namespace Shaykhullin.Activator.Core
{
	internal class Activator : IActivator
	{
		private readonly IArrayPool arrayPool;

		public Activator(IArrayPool arrayPool)
		{
			this.arrayPool = arrayPool;
		}

		public TObject Create<TObject, TArg1>(TArg1 arg1)
		{
			return Create<TObject>(typeof(TObject), arg1);
		}

		public object Create<TArg1>(Type type, TArg1 arg1)
		{
			var args = arrayPool.GetArrayExact<object>(1);
			args[0] = arg1;

			var instance = Create(type, args);
			arrayPool.ReleaseArray(args);

			return instance;
		}

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
