using System;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections.Generic;

using Shaykhullin.ArrayPool;

namespace Shaykhullin.Activator.Core
{
	internal class Activator : IActivator
	{
		private static readonly Dictionary<Type, Func<object[], object>> Cache = new Dictionary<Type, Func<object[], object>>();
			
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
			return (TObject)Create(typeof(TObject), args);
		}

		public object Create(Type type, params object[] args)
		{
			if (Cache.TryGetValue(type, out var func))
			{
				return func(args);
			}

			var ctor = type.GetConstructors()[0];
			var activatorDelegate = CreateDelegate(ctor);
			
			Cache.Add(type, activatorDelegate);
			return activatorDelegate(args);
		}
		
		private static Func<object[], object> CreateDelegate(ConstructorInfo constructor)
		{
			var parameter = Expression.Parameter(typeof(object[]), "args");

			return (Func<object[], object>)Expression.Lambda(
				typeof(Func<object[], object>),
					Expression.New(
						constructor, 
						constructor.GetParameters()
							.Select(ArgumentExpression)
							.Select(x => x.Invoke(parameter))
							.Cast<Expression>()
							.ToArray()), 
					parameter)
				.Compile();
		}

		private static Func<ParameterExpression, UnaryExpression> ArgumentExpression(ParameterInfo parameterInfo, int index) => 
			parameterExpression => 
				Expression.Convert(
					Expression.ArrayIndex(
						parameterExpression, 
						Expression.Constant(index)), 
					parameterInfo.ParameterType);
	}
}
