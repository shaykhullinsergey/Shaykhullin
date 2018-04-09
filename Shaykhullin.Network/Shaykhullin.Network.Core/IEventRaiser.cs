using System.Threading.Tasks;

namespace Network.Core
{
	public interface IEventRaiser
	{
		Task Raise(IPayload payload);
	}
}