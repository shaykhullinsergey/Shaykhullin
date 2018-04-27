using Shaykhullin.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Shaykhullin.Network.Core
{
	public class HandlerCollection
	{
		private readonly Dictionary<Type, List<Type>> handlers = new Dictionary<Type, List<Type>>();
		private readonly IContainerConfig config;

		public HandlerCollection(IContainerConfig config)
		{
			this.config = config;
		}

		public void Add<TData, TCommand, THandler>()
			where TCommand : ICommand<TData>
			where THandler : IHandler<TData, TCommand>
		{
			var command = typeof(TCommand);
			var handler = typeof(THandler);
			
			if (handlers.TryGetValue(command, out var list))
			{
				list.Add(handler);
			}
			else
			{
				config.Register<THandler>();
				handlers.Add(command, new List<Type> { handler });
			}
		}

		public IList<Type> GetHandlers(Type command)
		{
			if(!handlers.TryGetValue(command, out var list))
			{
				list = new List<Type>();
				handlers.Add(command, list);
			}

			return list;
		}
	}
}