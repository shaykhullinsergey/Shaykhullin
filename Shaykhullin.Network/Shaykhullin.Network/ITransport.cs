using System;
using System.Threading.Tasks;

namespace Shaykhullin.Network.Core
{
	public interface ITransport : IDisposable
	{
		Task Connect();
		
		Packet ReadPacket();
		void WritePacket(Packet packet);
		
		Task<Packet> ReadPacketAsync();
		Task WritePacketAsync(Packet packet);
	}
}