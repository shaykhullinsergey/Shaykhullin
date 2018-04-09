using System.Threading.Tasks;

namespace Shaykhullin.Network.Core
{
	public interface IServer : INode
	{
		Task Run();
	}
}