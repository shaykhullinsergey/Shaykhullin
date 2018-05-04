namespace Shaykhullin.Network.Core
{
  internal class Packet : IPacket
  {
		public byte Id { get; set; }
    public ushort Order { get; set; }
    public byte Length { get; set; }
    public bool IsLast { get; set; }
    public byte[] Buffer { get; set; }
	}
}
