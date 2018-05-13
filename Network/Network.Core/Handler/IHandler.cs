namespace Network
{
	public interface IHandler<TData, TCommand>
		where TCommand : ICommand<TData>
	{
		void Execute(in TCommand command);
	}
}