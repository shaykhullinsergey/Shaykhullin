using System.Collections.Generic;

namespace Shaykhullin.Network.Core
{
	public interface IPacketsComposer
	{
		byte[] GetBuffer();
		void ReleaseBuffer(byte[] buffer);
		Message GetMessage(IList<Packet> packets);
		Packet GetPacket(byte[] buffer);
		Packet[] GetPackets(Message message);
	}
}