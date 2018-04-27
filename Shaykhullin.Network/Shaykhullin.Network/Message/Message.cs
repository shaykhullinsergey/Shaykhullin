namespace Shaykhullin.Network.Core
{
	internal class Message : IMessage
	{
		public int CommandId { get; set; }
		public byte[] Data { get; set; }
	}
}