using System;

namespace Network
{
	public class DisconnectInfo
	{
		internal DisconnectInfo(string message, Exception exception)
		{
			Reason = message;
			Exception = exception;
		}

		public string Reason { get; }
		public Exception Exception { get; set; }
	}
}
