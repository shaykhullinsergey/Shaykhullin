using System.Threading.Tasks;

namespace Shaykhullin.Network
{
	public interface IHandler<TData, TCommand>
		where TCommand : ICommand<TData>
	{
		Task Execute(TCommand command);
	}
}