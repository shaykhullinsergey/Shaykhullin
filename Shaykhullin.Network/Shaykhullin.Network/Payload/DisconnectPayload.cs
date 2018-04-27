using System;

namespace Shaykhullin.Network.Core
{
	internal class DisconnectPayload : Payload
	{
		private static readonly Type DisonnectCommandType = typeof(Disconnect);
		
		public DisconnectPayload(string message, Exception exception = null)
		{
			CommandType = DisonnectCommandType;
			Data = new DisconnectInfo(message, exception);
		}
	}
}