using Network.Core;

namespace Network
{
	public interface IEvent<out TData>
	{
		IConnection Connection { get; }
		TData Message { get; }
	}
}