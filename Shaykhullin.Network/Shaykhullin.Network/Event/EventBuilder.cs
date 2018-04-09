using Shaykhullin.DependencyInjection;

namespace Network.Core
{
	internal class EventBuilder : IEventBuilder
	{
		private readonly IContainerConfig config;

		public EventBuilder(IContainerConfig config)
		{
			this.config = config;
		}
		
		public IHandlerBuilder<TEvent> When<TEvent>()
			where TEvent : class, IEvent<object>
		{
			return new HandlerBuilder<TEvent>(config);
		}
	}
}
