using System.IO;

namespace Shaykhullin.Network.Core
{
	public interface IEncryption
	{
		void Encrypt(Stream stream);
		void Decrypt(Stream stream);
	}
}
