using System.Threading.Tasks;

namespace Shaykhullin.Network.Core
{
	public interface IMessageComposer
	{
		Message GetMessage<TData>(IPayload<TData> payload);
		IPayload<TData> GetPayload<TData>(Message message);
	}
}