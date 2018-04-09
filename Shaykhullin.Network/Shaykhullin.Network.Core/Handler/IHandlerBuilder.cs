namespace Network.Core
{
	public interface IHandlerBuilder<TEvent>
		where TEvent : class, IEvent<object> 
	{
		void Use<THandler>()
			where THandler : class, IHandler<TEvent>;
	}
}
