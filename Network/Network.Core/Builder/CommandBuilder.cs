using System;

namespace Network.Core
{
	public readonly struct CommandBuilder<TData> : IDisposable
	{
		private readonly CommandConfiguration configuration;
		
		internal CommandBuilder(CommandConfiguration configuration)
		{
			this.configuration = configuration;
		}

		public HandlerBuilder<TData, TCommand> From<TCommand>()
			where TCommand : ICommand<TData>
		{
			var from = configuration.EnsureCommand(typeof(TCommand));
			return new HandlerBuilder<TData, TCommand>(from);
		}

		public void Dispose()
		{
		}
	}
}