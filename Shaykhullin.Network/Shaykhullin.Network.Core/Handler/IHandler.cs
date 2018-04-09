using System.Threading.Tasks;

namespace Network
{
	public interface IHandler<TEvent>
		where TEvent : IEvent<object>
	{
		Task Execute(TEvent @event);
	}
}