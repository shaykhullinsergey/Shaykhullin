namespace Shaykhullin.Network.Core
{
	public interface IDataBuilder
	{
		IEventBuilder<TData> AddType<TData>();
	}

	public interface IEventBuilder<TData>
	{
		IHandlerBuilder<TData, TEvent> FromEvent<TEvent>()
			where TEvent : IEvent<TData>;
	}
}
