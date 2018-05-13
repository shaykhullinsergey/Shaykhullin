using System;
using System.Net.Sockets;
using Network.Core;

namespace Network
{
	public readonly struct Client : IDisposable
	{
		private readonly Configuration configuration;
		
		internal Client(Configuration configuration)
		{
			this.configuration = configuration;
		}

		public Connection Connect(string host, int port)
		{
			var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
			socket.Connect(host, port);
			
			return new Connection(socket, configuration);
		}

		public void Dispose()
		{
		}
	}
}