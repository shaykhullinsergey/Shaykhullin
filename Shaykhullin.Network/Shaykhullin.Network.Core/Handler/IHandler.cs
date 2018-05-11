using System.Threading.Tasks;

namespace Shaykhullin.Network
{
	public interface IAsyncHandler<TData, TCommand>
		where TCommand : ICommand<TData>
	{
		Task Execute(TCommand command);
	}
	
	public interface IHandler<TData, TCommand>
		where TCommand : ICommand<TData>
	{
		void Execute(TCommand command);
	}
}