using System;
using System.Threading.Tasks;

namespace Shaykhullin.Network.Core
{
	public interface IClient : IApplication, IDisposable
	{
		Task<IConnection> Connect();
	}
}
