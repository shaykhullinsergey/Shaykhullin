namespace Network.Core
{
	public interface ICompression
	{
		byte[] Compress(byte[] data);
		byte[] Decompress(byte[] data);
	}
}
