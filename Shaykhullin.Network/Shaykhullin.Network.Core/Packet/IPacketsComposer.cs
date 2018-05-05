using System.Collections.Generic;

namespace Shaykhullin.Network.Core
{
	public interface IPacketsComposer
	{
		byte[] GetBuffer();
		void ReleaseBuffer(byte[] buffer);
		IMessage GetMessage(IList<IPacket> packets);
		IPacket GetPacket(byte[] buffer);
		IPacket[] GetPackets(IMessage message);
	}
}