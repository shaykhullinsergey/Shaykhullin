using System;

namespace Shaykhullin.Network.Core
{
	internal class ConnectPayload : Payload<ConnectInfo>
	{
		private static readonly Type ConnectCommandType = typeof(Connect);
		
		public ConnectPayload()
		{
			CommandType = ConnectCommandType;
			Data = new ConnectInfo();
		}
	}
}