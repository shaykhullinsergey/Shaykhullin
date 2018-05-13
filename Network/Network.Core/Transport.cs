using System;
using System.Net.Sockets;

namespace Network
{
	public class Transport : IDisposable
	{
		private readonly Socket socket;
		private readonly CommandRaiser raiser;

		public Transport(Socket socket, CommandRaiser raiser)
		{
			this.socket = socket;
			this.raiser = raiser;
		}

		public byte[] Receive()
		{
			var buffer = new byte[256];

			var receive = socket.Receive(buffer, 0, buffer.Length, SocketFlags.None);

			if (receive == 0)
			{
				raiser.Raise(); // Disconnected
				throw new OperationCanceledException();
			}
			
			return buffer;
		}

		public void Send(byte[] buffer)
		{
			try
			{
				socket.Send(buffer, 0, buffer.Length, SocketFlags.None);
			}
			catch (Exception e)
			{
				raiser.Raise(); // Closed
				throw;
			}
		}

		public void Dispose()
		{
			socket?.Dispose();
		}
	}
}