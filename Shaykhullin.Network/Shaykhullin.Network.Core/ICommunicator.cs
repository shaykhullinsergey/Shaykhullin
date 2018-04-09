using System.Threading.Tasks;

namespace Shaykhullin.Network.Core
{
	public interface ICommunicator
	{
		bool IsConnected { get; }
		Task Connect();
		Task<IPacket> Receive();
		Task Send(IPacket packet);
	}
}