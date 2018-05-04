using System;
using System.Collections.Generic;

namespace Shaykhullin.Network.Core
{
	public interface ICommandHolder
	{
		Type GetCommand(int id);
		int GetCommand(Type command);
		IList<IHandlerDto> GetHandlers(IPayload payload);
		Type GetGenericArgument(Type command);
	}
}
