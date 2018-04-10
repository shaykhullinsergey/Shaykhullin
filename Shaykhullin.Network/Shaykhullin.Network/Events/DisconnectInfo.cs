using System;

namespace Shaykhullin.Network
{
	public class DisconnectInfo
	{
		internal DisconnectInfo(string message, Exception exception = null)
		{
			Reason = message;
			Exception = exception;
		}

		public string Reason { get; }
		public Exception Exception { get; set; }
	}
}
