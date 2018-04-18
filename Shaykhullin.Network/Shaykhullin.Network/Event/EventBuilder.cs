using Shaykhullin.DependencyInjection;

namespace Shaykhullin.Network.Core
{
	internal class EventBuilder<TData> : IEventBuilder<TData>
	{
		private readonly IContainerConfig config;

		public EventBuilder(IContainerConfig config)
		{
			this.config = config;
		}
		
		public IHandlerBuilder<TData, TEvent> From<TEvent>()
			where TEvent : IEvent<TData>
		{
			return new HandlerBuilder<TData, TEvent>(config);
		}
	}
}
