using Shaykhullin.DependencyInjection;
using Shaykhullin.Serializer;

namespace Shaykhullin.Network.Core
{
	internal class HandlerBuilder<TData, TEvent> : IHandlerBuilder<TData, TEvent>
		where TEvent : IEvent<TData>
	{
		private readonly IContainerConfig config;

		public HandlerBuilder(IContainerConfig config)
		{
			this.config = config;
		}

		public void With<THandler>() where THandler 
			: IHandler<TData, TEvent>
		{
			using (var container = config.Create())
			{
				container.Resolve<ISerializerConfig>()
					.Match<TData>();
	
				container.Resolve<EventCollection>()
					.Add<TData, TEvent>();
	
				container.Resolve<HandlerCollection>()
					.Add<TData, TEvent, THandler>();
			}
		}
	}
}
