namespace Shaykhullin.Network.Core
{
	public interface IHandlerBuilder<TData, TEvent>
		where TEvent : IEvent<TData> 
	{
		void With<THandler>()
			where THandler : IHandler<TData, TEvent>;
	}
}
