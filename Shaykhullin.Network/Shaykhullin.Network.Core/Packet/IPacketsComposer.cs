using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shaykhullin.Network.Core
{
	public interface IPacketsComposer
	{
		Task<byte[]> GetBuffer();
		Task<byte[]> GetBytes(IPacket packet);
		Task<IMessage> GetMessage(IList<IPacket> packets);
		Task<IPacket> GetPacket(byte[] data);
		Task<IPacket[]> GetPackets(IMessage message);
	}
}