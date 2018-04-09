namespace Network.Core
{
	public interface IEncryptionBuilder
	{
		void UseEncryption<TEncryption>()
			where TEncryption : IEncryption;
	}
}
