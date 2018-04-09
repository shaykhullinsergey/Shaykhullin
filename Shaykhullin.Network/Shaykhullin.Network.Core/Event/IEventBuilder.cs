namespace Network.Core
{
	public interface IEventBuilder
	{
		IHandlerBuilder<TEvent> When<TEvent>()
			where TEvent : class, IEvent<object>;
	}
}
