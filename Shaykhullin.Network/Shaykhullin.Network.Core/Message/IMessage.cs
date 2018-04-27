namespace Shaykhullin.Network.Core
{
	public interface IMessage
	{
		int CommandId { get; }
		byte[] Data { get; }
	}
}