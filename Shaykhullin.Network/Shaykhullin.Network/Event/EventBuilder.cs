using Shaykhullin.DependencyInjection;

namespace Shaykhullin.Network.Core
{
	internal class EventBuilder<TData> : IEventBuilder<TData>
	{
		private readonly IContainer container;

		public EventBuilder(IContainer container)
		{
			this.container = container;
		}
		
		public IHandlerBuilder<TData, TEvent> From<TEvent>()
			where TEvent : IEvent<TData>
		{
			return new HandlerBuilder<TData, TEvent>(container);
		}
	}
}
