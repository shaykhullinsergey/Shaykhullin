using System;
using System.Threading.Tasks;

namespace Shaykhullin.Network.Core
{
	public interface ICommunicator : IDisposable
	{
		bool IsConnected { get; }
		Task Connect();
		Task<IPacket> Receive();
		Task Send(IPacket packet);
	}
}