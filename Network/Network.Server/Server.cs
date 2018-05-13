using System;
using System.Net;
using System.Net.Sockets;
using Network.Core;

namespace Network
{
	public readonly struct Server : IDisposable
	{
		private readonly Configuration configuration;
		
		internal Server(Configuration configuration)
		{
			this.configuration = configuration;
		}

		public void Listen(string host, int port, int backlog = 10)
		{
			var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
			
			socket.Bind(new IPEndPoint(IPAddress.Parse(host), port));
			socket.Listen(backlog);

			while (true)
			{
				var client = socket.Accept();
				client.ReceiveBufferSize = client.SendBufferSize = 256;

				new Connection(client, configuration);
			}
		}

		public void Dispose()
		{
		}
	}
}