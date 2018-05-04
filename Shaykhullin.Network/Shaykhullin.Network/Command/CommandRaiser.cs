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

		public async Task RaiseCommand(IPayload payload)
		{
			using (var container = config.Create())
			{
				var handlerDtos = container
					.Resolve<ICommandHolder>()
					.GetHandlers(payload);

				using (var scope = config.CreateScope())
				{
					var data = payload.Data;
					scope.Register(payload.Data.GetType())
						.ImplementedBy(c => data)
						.As<Singleton>();

					using (var scopeContainer = scope.Create())
					{
						var command = scopeContainer.Resolve(payload.CommandType);

						for (var i = 0; i < handlerDtos.Count; i++)
						{
							var instanse = scopeContainer.Resolve(handlerDtos[i].HandlerType);

							try
							{
								await handlerDtos[i].ExecuteMethod(instanse, command);
							}
							catch (Exception exception)
							{
								await RaiseCommand(new ErrorPayload($"Handler {handlerDtos[i].GetType()}", exception));
							}
						}
					}
				}
			}
		}
	}
}