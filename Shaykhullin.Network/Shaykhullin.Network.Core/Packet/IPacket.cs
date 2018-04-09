namespace Network.Core
{
	public interface IPacket
	{
		byte Id { get; }
		ushort Order { get; }
		byte Length { get; }
		bool End { get; }
		byte[] Chunk { get; }
	}
}