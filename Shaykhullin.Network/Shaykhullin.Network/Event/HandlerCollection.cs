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

		public void Add<TData, TEvent, THandler>()
			where TEvent : IEvent<TData>
			where THandler : IHandler<TData, TEvent>
		{
			var @event = typeof(TEvent);
			var handler = typeof(THandler);
			
			if (handlers.TryGetValue(@event, out var list))
			{
				list.Add(handler);
			}
			else
			{
				config.Register<THandler>();
				handlers.Add(@event, new List<Type> { handler });
			}
		}

		public IList<Type> GetHandlers(Type @event)
		{
			if(!handlers.TryGetValue(@event, out var list))
			{
				list = new List<Type>();
				handlers.Add(@event, list);
			}

			return list;
		}
	}
}