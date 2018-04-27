using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shaykhullin.Network.Core
{
	public interface IPacketsComposer
	{
		byte[] GetBuffer();
		byte[] GetBytes(IPacket packet);
		IMessage GetMessage(IList<IPacket> packets);
		IPacket GetPacket(byte[] data);
		Task<IPacket[]> GetPackets(IMessage message);
	}
}