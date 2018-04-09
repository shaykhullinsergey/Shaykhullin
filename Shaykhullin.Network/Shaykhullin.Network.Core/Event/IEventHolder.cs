using System;
using System.Collections.Generic;

namespace Shaykhullin.Network.Core
{
	public interface IEventHolder
	{
		Type GetEvent(int id);
		int GetEvent(Type @event);
		IList<Type> GetHandlers(IPayload payload);
	}
}
