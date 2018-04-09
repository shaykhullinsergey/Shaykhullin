using System.Threading.Tasks;

namespace Shaykhullin.Network.Core
{
	public interface ISendBuilder<TData>
	{
		Task To<TEvent>()
			where TEvent : IEvent<TData>;
	}
}
