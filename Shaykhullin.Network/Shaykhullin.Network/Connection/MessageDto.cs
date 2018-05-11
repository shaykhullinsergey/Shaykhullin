using System.Collections.Generic;

namespace Shaykhullin.Network.Core
{
	internal class MessageDto
	{
		public byte MessageId { get; }
		public IList<Packet> Packets { get; }

		public MessageDto(byte messageId, IList<Packet> packets)
		{
			MessageId = messageId;
			Packets = packets;
		}
	}
}