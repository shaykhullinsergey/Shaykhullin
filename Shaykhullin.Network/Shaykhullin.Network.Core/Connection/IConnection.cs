using Shaykhullin.Network.Core;

namespace Shaykhullin.Network
{
	public interface IConnection
	{
		ISendBuilder<TData> Send<TData>(TData data);
	}
}
