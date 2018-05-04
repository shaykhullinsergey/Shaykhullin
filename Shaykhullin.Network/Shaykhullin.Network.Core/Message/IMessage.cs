namespace Shaykhullin.Network.Core
{
	public interface IMessage
	{
		byte[] DataStreamBuffer { get; }
		int DataStreamLength { get; }
	}
}