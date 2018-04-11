using Shaykhullin.DependencyInjection;
using Shaykhullin.Serializer;

namespace Shaykhullin.Network.Core
{
	internal class HandlerBuilder<TData, TEvent> : IHandlerBuilder<TData, TEvent>
		where TEvent : IEvent<TData>
	{
		private readonly IContainer container;

		public HandlerBuilder(IContainer container)
		{
			this.container = container;
		}

		public void With<THandler>() where THandler 
			: IHandler<TData, TEvent>
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
