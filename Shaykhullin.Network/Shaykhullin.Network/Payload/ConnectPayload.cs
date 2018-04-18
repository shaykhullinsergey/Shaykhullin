namespace Shaykhullin.Network.Core
{
	internal class ConnectPayload : Payload
	{
		public ConnectPayload()
		{
			Event = typeof(Connect);
			Data = new ConnectInfo();
		}
	}
}