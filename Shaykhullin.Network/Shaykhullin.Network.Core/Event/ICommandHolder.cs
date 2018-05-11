using System;
using System.Collections.Generic;

namespace Shaykhullin.Network.Core
{
	public interface ICommandHolder
	{
		Type GetCommand(int id);
		int GetCommand(Type command);
		IList<IHandlerDto> GetAsyncHandlers<TData>(IPayload<TData> payload);
		IList<IHandlerDto> GetHandlers<TData>(IPayload<TData> payload);
		Type GetGenericArgument(Type command);
	}
}
