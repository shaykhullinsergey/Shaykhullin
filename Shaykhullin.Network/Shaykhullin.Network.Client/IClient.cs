using System.Threading.Tasks;

namespace Shaykhullin.Network.Core
{
	public interface IClient : INode
	{
		Task<IConnection> Connect();
	}
}
