using Shaykhullin.DependencyInjection;
using Shaykhullin.Serializer;

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

		public IHandlerBuilder<TData, TCommand> Call<THandler>() where THandler 
			: IHandler<TData, TCommand>
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
	}
}
