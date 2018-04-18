using System;

namespace Shaykhullin.Network
{
	public class ConnectInfo
	{
		public DateTime ConnectionTime { get; }

		public ConnectInfo()
		{
			ConnectionTime = DateTime.Now;
		}
	}
}
