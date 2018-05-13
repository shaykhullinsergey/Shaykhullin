using System;
using System.Collections.Generic;

namespace Network.Core
{
	internal class HandlerConfiguration
	{
		private readonly List<Type> handlers;

		public HandlerConfiguration()
		{
			handlers = new List<Type>();
		}

		public void EnsureHandler(Type handlerType)
		{
			handlers.Add(handlerType);
		}

		private IReadOnlyList<Type> Handlers()
		{
			return handlers;
		}
	}
}