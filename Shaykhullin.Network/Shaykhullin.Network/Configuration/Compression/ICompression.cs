using Shaykhullin.Stream;

namespace Shaykhullin.Network.Core
{
	public interface ICompression
	{
		void Compress(ValueStream stream);
		void Decompress(ValueStream stream);
	}
}
