using System.Threading.Tasks;

namespace Shaykhullin.Network.Core
{
	public interface IMessageComposer
	{
		Task<IMessage> GetMessage(IPayload payload);
		Task<IPayload> GetPayload(IMessage message);
	}
}