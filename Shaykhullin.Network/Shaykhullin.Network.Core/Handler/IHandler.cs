using System.Threading.Tasks;

namespace Shaykhullin.Network
{
	public interface IHandler<TData, TEvent>
		where TEvent : IEvent<TData>
	{
		Task Execute(TEvent @event);
	}
}