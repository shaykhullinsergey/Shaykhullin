namespace Network
{
	public class Disconnect : IEvent<DisconnectInfo>
	{
		public Disconnect(IConnection connection, DisconnectInfo message)
		{
			Connection = connection;
			Message = message;
		}

		public IConnection Connection { get; }
		public DisconnectInfo Message { get; }
	}
}
