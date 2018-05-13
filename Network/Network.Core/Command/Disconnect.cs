namespace Network
{
	public readonly struct DisonnectDto
	{
	}
	
	public readonly struct Disconnect : ICommand<DisonnectDto>
	{
		public Disconnect(Connection connection, DisonnectDto data)
		{
			Connection = connection;
			Data = data;
		}
		
		public Connection Connection { get; }
		public DisonnectDto Data { get; }
	}
}