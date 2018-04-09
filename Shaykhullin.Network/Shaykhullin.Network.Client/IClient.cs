using System.Threading.Tasks;

namespace Network.Core
{
	public interface IClient : INode
	{
		Task<IConnection> Connect();
	}
}
