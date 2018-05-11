using Shaykhullin.Serializer;
using Shaykhullin.DependencyInjection;

namespace Shaykhullin.Network.Core
{
	public struct HandlerBuilder<TData, TCommand>
		where TCommand : ICommand<TData>
	{
		private readonly IContainerConfig config;

		internal HandlerBuilder(IContainerConfig config)
		{
			this.config = config;
		}

		public HandlerBuilder<TData, TCommand> Call<THandler>() 
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

		public HandlerBuilder<TData, TCommand> CallAsync<THandler>() 
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
