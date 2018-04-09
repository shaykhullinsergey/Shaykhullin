using System.Threading.Tasks;

namespace Network.Core
{
	public interface IServer : INode
	{
		Task Run();
	}
}