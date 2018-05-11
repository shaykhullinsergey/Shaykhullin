using System;
using System.Collections.Generic;
using Shaykhullin.DependencyInjection;

namespace Shaykhullin.Network.Core
{
	internal class CommandHolder : ICommandHolder
	{
		private readonly CommandCollection commandCollention;
		private readonly HandlerCollection handlerCollection;

		public CommandHolder(IContainer container)
		{
			commandCollention = container.Resolve<CommandCollection>();
			handlerCollection = container.Resolve<HandlerCollection>();
		}

		public Type GetCommand(int id)
		{
			return commandCollention.GetCommand(id);
		}

		public int GetCommand(Type command)
		{
			return commandCollention.GetCommand(command);
		}

		public IList<IHandlerDto> GetAsyncHandlers<TData>(IPayload<TData> payload)
		{
			return handlerCollection.GetAsyncHandlers(payload.CommandType);
		}
		
		public IList<IHandlerDto> GetHandlers<TData>(IPayload<TData> payload)
		{
			return handlerCollection.GetHandlers(payload.CommandType);
		}

		public Type GetGenericArgument(Type command)
		{
			return commandCollention.GetGenericArgument(command);
		}
	}
}