using Shaykhullin.Serializer;
using Shaykhullin.DependencyInjection;

namespace Shaykhullin.Network.Core
{
	internal class HandlerBuilder<TData, TCommand> : IHandlerBuilder<TData, TCommand>
		where TCommand : ICommand<TData>
	{
		private readonly IContainerConfig config;

		public HandlerBuilder(IContainerConfig config)
		{
			this.config = config;
		}

		public IHandlerBuilder<TData, TCommand> Call<THandler>() 
			where THandler : IHandler<TData, TCommand>
		{
			using (var container = config.Create())
			{
				container.Resolve<ISerializerConfig>()
					.Match<TData>();
	
				container.Resolve<CommandCollection>()
					.Add<TData, TCommand>();
	
				container.Resolve<HandlerCollection>()
					.Add<TData, TCommand, THandler>();

				return this;
			}
		}

		public IHandlerBuilder<TData, TCommand> CallAsync<THandler>() 
			where THandler : IAsyncHandler<TData, TCommand>
		{
			using (var container = config.Create())
			{
				container.Resolve<ISerializerConfig>()
					.Match<TData>();
	
				container.Resolve<CommandCollection>()
					.Add<TData, TCommand>();
	
				container.Resolve<HandlerCollection>()
					.AddAsync<TData, TCommand, THandler>();

				return this;
			}
		}
	}
}
