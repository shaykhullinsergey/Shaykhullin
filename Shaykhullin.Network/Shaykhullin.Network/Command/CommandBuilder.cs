using Shaykhullin.DependencyInjection;

namespace Shaykhullin.Network.Core
{
	public struct CommandBuilder<TData>
	{
		private readonly IContainerConfig config;

		internal CommandBuilder(IContainerConfig config)
		{
			this.config = config;
		}
		
		public HandlerBuilder<TData, TCommand> In<TCommand>()
			where TCommand : ICommand<TData>
		{
			return new HandlerBuilder<TData, TCommand>(config);
		}
	}
}
