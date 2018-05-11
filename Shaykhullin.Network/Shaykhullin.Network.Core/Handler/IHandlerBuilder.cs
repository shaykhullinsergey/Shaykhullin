namespace Shaykhullin.Network.Core
{
	public interface IHandlerBuilder<TData, TCommand>
		where TCommand : ICommand<TData> 
	{
		IHandlerBuilder<TData, TCommand> Call<THandler>()
			where THandler : IHandler<TData, TCommand>;
		
		IHandlerBuilder<TData, TCommand> CallAsync<THandler>()
			where THandler : IAsyncHandler<TData, TCommand>;
	}
}
