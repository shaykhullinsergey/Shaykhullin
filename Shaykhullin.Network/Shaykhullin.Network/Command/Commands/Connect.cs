namespace Shaykhullin.Network
{
	public class Connect : ICommand<ConnectInfo>
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
