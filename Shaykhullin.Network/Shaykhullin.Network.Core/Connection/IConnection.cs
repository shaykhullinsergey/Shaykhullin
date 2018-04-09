using Network.Core;

namespace Network
{
	public interface IConnection
	{
		ISendBuilder<TData> Send<TData>(TData data);
	}
}
