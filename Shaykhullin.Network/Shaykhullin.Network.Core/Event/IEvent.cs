using Shaykhullin.Network.Core;

namespace Shaykhullin.Network
{
	public interface IEvent<out TData>
	{
		IConnection Connection { get; }
		TData Message { get; }
	}
}