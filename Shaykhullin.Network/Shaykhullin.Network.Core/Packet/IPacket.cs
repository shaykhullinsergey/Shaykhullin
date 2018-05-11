namespace Shaykhullin.Network.Core
{
	public readonly struct Packet
	{
		public byte MessageId => Buffer[0];
		public byte Length => Buffer[1];
		public bool IsLast => Buffer[2] == 1;
		public byte[] Buffer { get; }

		public Packet(byte[] buffer)
		{
			Buffer = buffer;
		}
	}
}