using System.Threading.Tasks;

namespace Shaykhullin.Network.Core
{
	public interface IEventRaiser
	{
		Task Raise(IPayload payload);
	}
}