using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Shaykhullin.DependencyInjection;

namespace Shaykhullin.Network.Core
{
	internal class Transport : ITransport
	{
		private readonly object @lock = new object();
		private readonly byte[] isAliveBuffer = new byte[1];
		
		private readonly TcpClient tcpClient;
		private readonly IConfiguration configuration;
		private readonly ICommandRaiser commandRaiser;
		private readonly IPacketsComposer packetsComposer;

		public Transport(IContainer container)
		{
			tcpClient = container.Resolve<TcpClient>();
			configuration = container.Resolve<IConfiguration>();
			commandRaiser = container.Resolve<ICommandRaiser>();
			packetsComposer = container.Resolve<IPacketsComposer>();
		}

		public Task Connect()
		{
			if (!isAlive && !IsAlive)
			{
				lock (@lock)
				{
					if (!tcpClient.Connected)
					{
						tcpClient.ConnectAsync(configuration.Host, configuration.Port)
							.GetAwaiter().GetResult();
						
						commandRaiser.RaiseCommand(new ConnectPayload())
							.GetAwaiter().GetResult();
					}
				}
			}

			return Task.CompletedTask;
		}

		public async Task WritePacket(IPacket packet)
		{
			var data = packet.Buffer;

			try
			{
				tcpClient.GetStream().Write(data, 0, data.Length);
			}
			catch (Exception exception)
			{
				await commandRaiser.RaiseCommand(new DisconnectPayload("Connection closed", exception)).ConfigureAwait(false);
			}
		}

		public async Task<IPacket> ReadPacket()
		{
			var buffer = packetsComposer.GetBuffer();

			while (!isAlive && !IsAlive)
			{
				await Connect().ConfigureAwait(false);
			}

			var read = tcpClient.GetStream().Read(buffer, 0, buffer.Length);

			if (read == 0 && !IsAlive)
			{
				await commandRaiser.RaiseCommand(new DisconnectPayload("Connection closed")).ConfigureAwait(false);
				throw new OperationCanceledException();
			}

			return packetsComposer.GetPacket(buffer);
		}

		public async void Dispose()
		{
			tcpClient.Close();
			tcpClient.Dispose();
			await commandRaiser.RaiseCommand(new DisconnectPayload("Connection disposed")).ConfigureAwait(false);
		}

		private bool isAlive;
		private bool IsAlive
		{
			get
			{
				try
				{
					if (tcpClient?.Client?.Connected != true)
					{
						return false;
					}

					if (!tcpClient.Client.Poll(0, SelectMode.SelectRead))
					{
						return isAlive = true;
					}

					if (tcpClient.Client.Receive(isAliveBuffer, SocketFlags.Peek) == 0)
					{
						return false;
					}

					return isAlive = true;
				}
				catch
				{
					return false;
				}
			}
		}
	}
}