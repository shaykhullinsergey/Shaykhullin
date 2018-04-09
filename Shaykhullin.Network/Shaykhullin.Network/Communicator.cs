using System;
using System.Net.Sockets;
using System.Threading;
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
					if(!tcpClient.Connected)
					{
						tcpClient.ConnectAsync(configuration.Host, configuration.Port).Wait();
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
				await eventRaiser.Raise(new Payload
				{
					Event = typeof(Disconnect),
					Data = new DisconnectInfo("Connection closed", exception)
				}).ConfigureAwait(false);
			}
		}

		public async Task<IPacket> Receive()
		{
			var buffer = await packetsComposer.GetBuffer();

			while (!isConnected && !IsConnected)
			{
				await Connect().ConfigureAwait(false);
			}

			await tcpClient.GetStream().ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
			return await packetsComposer.GetPacket(buffer).ConfigureAwait(false);
		}

		private bool isConnected = false;
		public bool IsConnected
		{
			get
			{
				try
				{
					if (tcpClient != null && tcpClient.Client != null && tcpClient.Client.Connected)
					{
						if (tcpClient.Client.Poll(0, SelectMode.SelectRead))
						{
							byte[] buff = new byte[1];
							if (tcpClient.Client.Receive(buff, SocketFlags.Peek) == 0)
							{
								return false;
							}
							else
							{
								return isConnected = true;
							}
						}
						return isConnected = true;
					}
					else
					{
						return false;
					}
				}
				catch
				{
					return false;
				}
			}
		}
	}
}