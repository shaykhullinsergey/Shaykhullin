using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Shaykhullin.Network.Core
{
	internal class AsyncHandlerDto : IHandlerDto
	{
		private static readonly Dictionary<Type, Func<object, object, Task>> MethodCache =
			new Dictionary<Type, Func<object, object, Task>>();

		public Type HandlerType { get; }
		public Func<object, object, Task> ExecuteMethod { get; }

		public AsyncHandlerDto(Type handlerType)
		{
			HandlerType = handlerType;

			if (!MethodCache.TryGetValue(handlerType, out var executeMethod))
			{
				var execute = handlerType
					.GetMethod("Execute", BindingFlags.Instance | BindingFlags.Public);
				
				var magic = typeof(AsyncHandlerDto)
					.GetMethod("MagicMethod", BindingFlags.Static | BindingFlags.NonPublic)
					.MakeGenericMethod(handlerType);
				
				executeMethod = (Func<object, object, Task>)magic.Invoke(null, new object[] {execute});
				
				MethodCache.Add(handlerType, executeMethod);
			}

			ExecuteMethod = executeMethod;
		}

		static Func<T, object, Task> MagicMethod<T>(MethodInfo method)
		{
			var constructedHelper = typeof(AsyncHandlerDto)
				.GetMethod("MagicMethodHelper", BindingFlags.Static | BindingFlags.NonPublic)
				.MakeGenericMethod(typeof(T), method.GetParameters()[0].ParameterType);

			return (Func<T, object, Task>)constructedHelper.Invoke(null, new object[] {method});
		}

		static Func<object, object, Task> MagicMethodHelper<TTarget, TParam>(MethodInfo method)
		{
			var func = (Func<TTarget, TParam, Task>)Delegate.CreateDelegate
				(typeof(Func<TTarget, TParam, Task>), method);

			return (target, param) => func((TTarget)target, (TParam)param);
		}
	}
}