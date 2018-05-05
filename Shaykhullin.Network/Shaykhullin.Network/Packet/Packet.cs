namespace Shaykhullin.Network.Core
{
  internal class Packet : IPacket
	{
		public byte Id => Buffer[0];
		public byte Length => Buffer[1];
		public bool IsLast => Buffer[2] == 1;
    public byte[] Buffer { get; set; }
	}
}
