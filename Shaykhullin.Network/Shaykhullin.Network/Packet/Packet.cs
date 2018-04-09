namespace Network.Core
{
  internal class Packet : IPacket
  {
		public byte Id { get; set; }
    public ushort Order { get; set; }
    public byte Length { get; set; }
    public bool End { get; set; }
    public byte[] Chunk { get; set; }
	}
}
