using System;

namespace Shaykhullin.Network
{
	public class DisconnectInfo
	{
		internal DisconnectInfo(string message, Exception exception = null)
		{
			Reason = message;
			Exception = exception;
			DisconnectionTime = DateTime.Now;
		}

		public string Reason { get; }
		public Exception Exception { get; }
		public DateTime DisconnectionTime { get; }
	}
}
