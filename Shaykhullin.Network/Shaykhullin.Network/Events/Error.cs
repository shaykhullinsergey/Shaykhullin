using Network;

namespace Network
{
	public class Error : IEvent<ErrorInfo>
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
