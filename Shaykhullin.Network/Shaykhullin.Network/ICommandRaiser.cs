using System.Threading.Tasks;

namespace Shaykhullin.Network.Core
{
	public interface ICommandRaiser
	{
		Task RaiseCommand<TData>(IPayload<TData> payload);
	}
}