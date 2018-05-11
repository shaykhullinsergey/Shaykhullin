using System;
using System.Collections.Generic;
using System.Reflection;

namespace Shaykhullin.Network.Core
{
	internal class HandlerDto : IHandlerDto
	{
		private static readonly Dictionary<Type, Action<object, object>> MethodCache =
			new Dictionary<Type, Action<object, object>>();

		public Type HandlerType { get; }
		public Action<object, object> ExecuteMethod { get; }

		public HandlerDto(Type handlerType)
		{
			HandlerType = handlerType;

			if (!MethodCache.TryGetValue(handlerType, out var executeMethod))
			{
				var execute = handlerType
					.GetMethod("Execute", BindingFlags.Instance | BindingFlags.Public);
				
				var magic = typeof(HandlerDto)
					.GetMethod("MagicMethod", BindingFlags.Static | BindingFlags.NonPublic)
					.MakeGenericMethod(handlerType);
				
				executeMethod = (Action<object, object>)magic.Invoke(null, new object[] {execute});
				
				MethodCache.Add(handlerType, executeMethod);
			}

			ExecuteMethod = executeMethod;
		}

		static Action<T, object> MagicMethod<T>(MethodInfo method)
		{
			var constructedHelper = typeof(HandlerDto)
				.GetMethod("MagicMethodHelper", BindingFlags.Static | BindingFlags.NonPublic)
				.MakeGenericMethod(typeof(T), method.GetParameters()[0].ParameterType);

			return (Action<T, object>)constructedHelper.Invoke(null, new object[] {method});
		}

		static Action<object, object> MagicMethodHelper<TTarget, TParam>(MethodInfo method)
		{
			var action = (Action<TTarget, TParam>)Delegate.CreateDelegate
				(typeof(Action<TTarget, TParam>), method);

			return (target, param) => action((TTarget)target, (TParam)param);
		}
	}
}