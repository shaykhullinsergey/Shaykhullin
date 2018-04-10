namespace Shaykhullin.Network
{
	public class Connect : IEvent<ConnectInfo>
	{
		public Connect(IConnection connection, ConnectInfo message)
		{
			Connection = connection;
			Message = message;
		}

		public IConnection Connection { get; }
		public ConnectInfo Message { get; }
	}
}
