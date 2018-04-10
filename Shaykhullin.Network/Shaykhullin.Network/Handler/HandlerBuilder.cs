using Shaykhullin.DependencyInjection;

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

		public void CallHandler<THandler>() where THandler 
			: IHandler<TData, TEvent>
		{
			container.Resolve<EventCollection>()
				.Add<TData, TEvent>();

			container.Resolve<HandlerCollection>()
				.Add<TData, TEvent, THandler>();
		}
	}
}
