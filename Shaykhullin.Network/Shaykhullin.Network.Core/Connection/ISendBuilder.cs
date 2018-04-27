using System.Threading.Tasks;

namespace Shaykhullin.Network.Core
{
	public interface ISendBuilder<TData>
	{
		Task To<TCommand>()
			where TCommand : ICommand<TData>;
	}
}
