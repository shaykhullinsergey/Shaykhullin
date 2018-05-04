using System.IO;

namespace Shaykhullin.Network.Core
{
	public interface ICompression
	{
		void Compress(Stream stream);
		void Decompress(Stream stream);
	}
}
