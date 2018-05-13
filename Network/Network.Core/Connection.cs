using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Network.Core;

namespace Network
{
	public class Connection : IDisposable
	{
		private readonly CommandRaiser raiser;
		private readonly Transport transport;
		private readonly Configuration configuration;
		private readonly Task receive;

		internal Connection(Socket socket, Configuration configuration)
		{
			raiser = new CommandRaiser();
			transport = new Transport(socket, raiser);
			this.configuration = configuration;
			receive = Task.Run(() => Receive());
		}

		public SendBuilder<TData> Send<TData>(TData data)
		{
			return new SendBuilder<TData>(transport, data);
		}

		private void Receive()
		{
			while (true)
			{
				var buffer = transport.Receive();
				Console.WriteLine(buffer[0]);
			}
		}

		public void Dispose()
		{
			transport.Dispose();
		}
	}
}