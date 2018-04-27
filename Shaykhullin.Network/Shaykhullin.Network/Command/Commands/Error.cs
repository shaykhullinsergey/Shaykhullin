namespace Shaykhullin.Network
{
	public class Error : ICommand<ErrorInfo>
	{
		public Error(IConnection connection, ErrorInfo message)
		{
			Connection = connection;
			Message = message;
		}

		public IConnection Connection { get; }
		public ErrorInfo Message { get; }
	}
}
