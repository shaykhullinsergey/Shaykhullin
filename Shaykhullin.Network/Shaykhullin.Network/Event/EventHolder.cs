using System;
using System.Collections.Generic;
using Shaykhullin.DependencyInjection;

namespace Network.Core
{
	internal class EventHolder : IEventHolder
	{
		private readonly EventCollection eventCollention;
		private readonly HandlerCollection handlerCollection;

		public EventHolder(IContainer container)
		{
			eventCollention = container.Resolve<EventCollection>();
			handlerCollection = container.Resolve<HandlerCollection>();
		}

		public Type GetEvent(int id)
		{
			return eventCollention.GetEvent(id);
		}

		public int GetEvent(Type @event)
		{
			return eventCollention.GetEvent(@event);
		}

		public IList<Type> GetHandlers(IPayload payload)
		{
			return handlerCollection.GetHandlers(payload.Event);
		}
	}
}