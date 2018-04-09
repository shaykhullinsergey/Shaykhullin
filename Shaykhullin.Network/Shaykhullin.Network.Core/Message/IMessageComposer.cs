using System.Threading.Tasks;

namespace Network.Core
{
	public interface IMessageComposer
	{
		Task<IMessage> GetMessage(IPayload payload);
		Task<IPayload> GetPayload(IMessage message);
	}
}