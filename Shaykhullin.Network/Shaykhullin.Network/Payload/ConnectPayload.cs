using System;

namespace Shaykhullin.Network.Core
{
	internal class ConnectPayload : Payload
	{
		private static readonly Type ConnectCommandType = typeof(Connect);
		
		public ConnectPayload()
		{
			CommandType = ConnectCommandType;
			Data = new ConnectInfo();
		}
	}
}