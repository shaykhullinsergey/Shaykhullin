namespace Shaykhullin.Network.Core
{
	public interface ICommandBuilder<TData>
	{
		IHandlerBuilder<TData, TCommand> In<TCommand>()
			where TCommand : ICommand<TData>;
	}
}