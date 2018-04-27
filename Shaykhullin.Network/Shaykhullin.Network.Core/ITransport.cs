using System;
using System.Threading.Tasks;

namespace Shaykhullin.Network.Core
{
	public interface ITransport : IDisposable
	{
		Task Connect();
		Task<IPacket> ReadPacket();
		Task WritePacket(IPacket packet);
	}
}