using System;

namespace Shaykhullin.Network.Core
{
	internal class DisconnectPayload : Payload
	{
		public DisconnectPayload(string message, Exception exception = null)
		{
			Event = typeof(Disconnect);
			Data = new DisconnectInfo(message, exception);
		}
	}
}