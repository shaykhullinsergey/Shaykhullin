using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Shaykhullin.DependencyInjection;

namespace Shaykhullin.Network.Core
{
	internal class CommandRaiser : ICommandRaiser
	{
		private readonly IContainerConfig config;

		public CommandRaiser(IContainerConfig config)
		{
			this.config = config;
		}

		public Task RaiseCommand(IPayload payload)
		{
			using (var container = config.Create())
			{
				var handlerTypes = container.Resolve<ICommandHolder>()
					.GetHandlers(payload);

				var handlers = new List<Task>();
				
				using (var scope = config.CreateScope())
				{
					var data = payload.Data;
					scope.Register(payload.Data.GetType())
						.ImplementedBy(c => data)
						.As<Singleton>();

					using (var scopeContainer = scope.Create())
					{
						var command = new[] { scopeContainer.Resolve(payload.CommandType) };

						for (var i = 0; i < handlerTypes.Count; i++)
						{
							var instanse = scopeContainer.Resolve(handlerTypes[i]);

							handlers.Add((Task)handlerTypes[i].InvokeMember(
								"Execute",
								BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod,
								null,
								instanse,
								command));
						}
					}
				}

				return Task.WhenAll(handlers);
			}
		}
	}
}