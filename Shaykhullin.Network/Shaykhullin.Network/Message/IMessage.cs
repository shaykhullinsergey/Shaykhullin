namespace Shaykhullin.Network.Core
{
	public readonly struct Message
	{
		public byte[] Data { get; }
		public int Length { get; }

		public Message(byte[] data, int length)
		{
			Data = data;
			Length = length;
		}
	}
}