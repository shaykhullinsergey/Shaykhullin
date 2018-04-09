using System.Threading.Tasks;

namespace Network.Core
{
	public interface ISendBuilder<TData>
	{
		Task In<TEvent>()
			where TEvent : IEvent<TData>;
	}
}
