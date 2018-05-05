namespace Shaykhullin.Network.Core
{
  internal class Packet : IPacket
	{
		public byte Id => Buffer[0];
		public byte Length => Buffer[3];
		public bool IsLast => Buffer[4] == 1;
    public byte[] Buffer { get; set; }
	}
}
