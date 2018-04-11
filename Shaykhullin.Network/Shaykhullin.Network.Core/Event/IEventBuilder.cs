namespace Shaykhullin.Network.Core
{
	public interface IDataBuilder
	{
		IEventBuilder<TData> Match<TData>();
	}

	public interface IEventBuilder<TData>
	{
		IHandlerBuilder<TData, TEvent> From<TEvent>()
			where TEvent : IEvent<TData>;
	}
}
