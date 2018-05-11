using System.IO;
using Shaykhullin.Stream;

namespace Shaykhullin.Network.Core
{
	public interface IEncryption
	{
		void Encrypt(ValueStream stream);
		void Decrypt(ValueStream stream);
	}
}
