namespace Network
{
	public readonly struct ConnectDto
	{
	}
	
	public readonly struct Connect : ICommand<ConnectDto>
	{
		public Connect(Connection connection, ConnectDto data)
		{
			Connection = connection;
			Data = data;
		}
		
		public Connection Connection { get; }
		public ConnectDto Data { get; }
	}
}