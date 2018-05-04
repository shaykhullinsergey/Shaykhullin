using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shaykhullin.Network.Core
{
	public interface IPacketsComposer
	{
		byte[] GetBuffer();
		void ReleaseBuffer(byte[] buffer);
		IMessage GetMessage(IList<IPacket> packets);
		IPacket GetPacket(byte[] buffer);
		Task<IPacket[]> GetPackets(IMessage message);
	}
}