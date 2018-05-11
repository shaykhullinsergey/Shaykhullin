using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shaykhullin.DependencyInjection;

namespace Shaykhullin.Network.Core
{
	internal class CommandRaiser : ICommandRaiser
	{
		private readonly IContainerConfig config;
		private readonly ICommandHolder commandHolder;

		public CommandRaiser(IContainerConfig config)
		{
			this.config = config;

			using (var container = config.Create())
			{
				commandHolder = container.Resolve<ICommandHolder>();
			}
		}

		private void RaiseSyncCommands<TData>(IContainer container, object command, IPayload<TData> payload)
		{
			var handlers = commandHolder.GetHandlers(payload);
					
			for (var i = 0; i < handlers.Count; i++)
			{
				var instanse = container.Resolve(handlers[i].HandlerType);

				try
				{
					(handlers[i] as HandlerDto).ExecuteMethod(instanse, command);
				}
				catch (Exception exception)
				{
					if (payload is ErrorPayload)
					{
						throw;
					}
							
					RaiseCommand(new ErrorPayload($"Handler {handlers[i].GetType()}", exception)).GetAwaiter().GetResult();
				}
			}
		}

		private async Task RaiseAsyncCommands<TData>(
			IContainer container, 
			IList<IHandlerDto> handlers, 
			object command, 
			IPayload<TData> payload)
		{
			for (var i = 0; i < handlers.Count; i++)
			{
				var instanse = container.Resolve(handlers[i].HandlerType);

				try
				{
					await (handlers[i] as AsyncHandlerDto).ExecuteMethod(instanse, command);
				}
				catch (Exception exception)
				{
					if (payload is ErrorPayload)
					{
						throw;
					}
							
					await RaiseCommand(new ErrorPayload($"Handler {handlers[i].GetType()}", exception));
				}
			}
		}
		
		public Task RaiseCommand<TData>(IPayload<TData> payload)
		{
			using (var scope = config.CreateScope())
			{
				scope.Register(payload.Data.GetType())
					.ImplementedBy(payload.Data);

				using (var container = scope.Create())
				{
					var command = container.Resolve(payload.CommandType);
					
					RaiseSyncCommands(container, command, payload);
					
					var asyncHandlers = commandHolder.GetAsyncHandlers(payload);
					
					return asyncHandlers.Count == 0
						? Task.CompletedTask
						: RaiseAsyncCommands(container, asyncHandlers, command, payload);
				}
			}
		}
	}
}