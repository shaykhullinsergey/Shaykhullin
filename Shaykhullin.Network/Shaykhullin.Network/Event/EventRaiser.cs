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
			using (var container = config.Create())
			{
				var handlers = container.Resolve<IEventHolder>()
					.GetHandlers(payload);

				foreach (var handler in handlers)
				{
					using (var scope = config.CreateScope())
					{
						scope.Register<object>(payload.Data.GetType())
							.ImplementedBy(c => payload.Data)
							.As<Singleton>();

						using (var scopeContainer = scope.Create())
						{
							var instanse = scopeContainer.Resolve(handler);
							var @event = scopeContainer.Resolve(payload.Event);

							await ((Task)handler.InvokeMember(
								"Execute",
								BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod,
								null,
								instanse,
								new[]
								{
									@event
								})).ConfigureAwait(false);
						}
					}
				}
			}
		}
	}
}
