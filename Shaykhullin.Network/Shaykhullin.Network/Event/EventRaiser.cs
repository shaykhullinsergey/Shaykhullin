using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Shaykhullin.DependencyInjection;

namespace Shaykhullin.Network.Core
{
	internal class EventRaiser : IEventRaiser
	{
		private readonly IContainerConfig config;

		public EventRaiser(IContainerConfig config)
		{
			this.config = config;
		}

		public async Task Raise(IPayload payload)
		{
			var handlers = config.Container
				.Resolve<IEventHolder>()
				.GetHandlers(payload);

			foreach (var handler in handlers)
			{
				using (var scope = config.Scope())
				{
					scope.Register<object>(payload.Data.GetType())
						.ImplementedBy(c => payload.Data)
						.As<Singleton>();

					var container = scope.Container;
				
					var instanse = container.Resolve(handler);
					var @event = container.Resolve(payload.Event);

					await ((Task)handler.InvokeMember(
						"Execute",
						BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod,
						null, 
						instanse, 
						new[] { @event })).ConfigureAwait(false);
				}
			}
		}
	}
}
