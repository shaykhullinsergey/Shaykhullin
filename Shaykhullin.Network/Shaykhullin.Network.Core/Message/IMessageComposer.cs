using System.Threading.Tasks;

namespace Shaykhullin.Network.Core
{
	public interface IMessageComposer
	{
		IMessage GetMessage(IPayload payload);
		IPayload GetPayload(IMessage message);
	}
}