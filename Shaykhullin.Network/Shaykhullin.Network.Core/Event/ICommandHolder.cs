using System;
using System.Collections.Generic;

namespace Shaykhullin.Network.Core
{
	public interface ICommandHolder
	{
		Type GetCommand(int id);
		int GetCommand(Type command);
		IList<Type> GetHandlers(IPayload payload);
		Type GetGenericArgument(Type command);
	}
}
