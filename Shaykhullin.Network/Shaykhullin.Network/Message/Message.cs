namespace Shaykhullin.Network.Core
{
	internal class Message : IMessage
	{
		public int EventId { get; set; }
		public byte[] Data { get; set; }
	}
}