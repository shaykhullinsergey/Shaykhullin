using Shaykhullin.DependencyInjection;

namespace Shaykhullin.Network.Core
{
	internal class CommandBuilder<TData> : ICommandBuilder<TData>
	{
		private readonly IContainerConfig config;

		public CommandBuilder(IContainerConfig config)
		{
			this.config = config;
		}
		
		public IHandlerBuilder<TData, TCommand> In<TCommand>()
			where TCommand : ICommand<TData>
		{
			return new HandlerBuilder<TData, TCommand>(config);
		}
	}
}
