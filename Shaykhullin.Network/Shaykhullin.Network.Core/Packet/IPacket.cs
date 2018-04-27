namespace Shaykhullin.Network.Core
{
	public interface IPacket
	{
		byte Id { get; }
		ushort Order { get; }
		byte Length { get; }
		bool IsLast { get; }
		byte[] Chunk { get; }
	}
}