namespace Shaykhullin.Network.Core
{
	public interface IEncryption
	{
		byte[] Encrypt(byte[] data);
		byte[] Decrypt(byte[] data);
	}
}
