using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Shaykhullin.DependencyInjection;

namespace Shaykhullin.Network.Core
{
	internal class Communicator : ICommunicator
	{
		private readonly object @lock = new object();
		private readonly TcpClient tcpClient;
		private readonly IPacketsComposer packetsComposer;
		private readonly IConfiguration configuration;
		private readonly IEventRaiser eventRaiser;

		public Communicator(IContainer container)
		{
			tcpClient = container.Resolve<TcpClient>();
			packetsComposer = container.Resolve<IPacketsComposer>();
			configuration = container.Resolve<IConfiguration>();
			eventRaiser = container.Resolve<IEventRaiser>();
		}

		public Task Connect()
		{
			if (!isConnected && !IsConnected)
			{
				lock (@lock)
				{
					if (!tcpClient.Connected)
					{
						tcpClient.ConnectAsync(configuration.Host, configuration.Port).Wait();
						eventRaiser.Raise(new ConnectPayload()).Wait();
					}
				}
			}

			return Task.CompletedTask;
		}

		public async Task Send(IPacket packet)
		{
			var data = await packetsComposer.GetBytes(packet).ConfigureAwait(false);

			try
			{
				await tcpClient.GetStream().WriteAsync(data, 0, data.Length).ConfigureAwait(false);
			}
			catch (Exception exception)
			{
				await eventRaiser.Raise(new DisconnectPayload("Connection closed", exception)).ConfigureAwait(false);
			}
		}

		public async Task<IPacket> Receive()
		{
			var buffer = await packetsComposer.GetBuffer().ConfigureAwait(false);

			while (!isConnected && !IsConnected)
			{
				await Connect().ConfigureAwait(false);
			}

			var read = await tcpClient.GetStream().ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);

			if (read == 0 && !IsConnected)
			{
				await eventRaiser.Raise(new DisconnectPayload("Connection closed")).ConfigureAwait(false);
				throw new OperationCanceledException();
			}

			return await packetsComposer.GetPacket(buffer).ConfigureAwait(false);
		}

		public async void Dispose()
		{
			tcpClient.Close();
			tcpClient.Dispose();
			await eventRaiser.Raise(new DisconnectPayload("Connection disposed")).ConfigureAwait(false);
		}

		private bool isConnected;

		public bool IsConnected
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
						return isConnected = true;
					}

					var buffer = new byte[1];

					if (tcpClient.Client.Receive(buffer, SocketFlags.Peek) == 0)
					{
						return false;
					}

					return isConnected = true;
				}
				catch
				{
					return false;
				}
			}
		}
	}
}