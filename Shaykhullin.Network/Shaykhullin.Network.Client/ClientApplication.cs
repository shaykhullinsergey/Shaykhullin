using System.Net.Sockets;
using System.Threading.Tasks;

using Shaykhullin.Network.Core;
using Shaykhullin.DependencyInjection;

namespace Shaykhullin.Network
{
	internal class ClientApplication : IClient
	{
		private readonly IContainerConfig config;

		public ClientApplication(IContainerConfig config)
		{
			this.config = config;
		}

		public async Task<IConnection> Connect()
		{
			var tcpClient = new TcpClient
			{
				NoDelay = true,
				ReceiveTimeout = 0,
				SendTimeout = 0,
				ReceiveBufferSize = 256,
				SendBufferSize = 256
			};

			config.Register<TcpClient>()
				.ImplementedBy(c => tcpClient)
				.As<Singleton>();

			var container = config.Create();

			config.Register<IContainer>()
				.ImplementedBy(c => c)
				.As<Singleton>();

			var connection = container.Resolve<IConnection>();

			config.Register<IConnection>()
				.ImplementedBy(c => connection)
				.As<Singleton>();

			var communicator = container.Resolve<ITransport>();

			await communicator.Connect().ConfigureAwait(false);

			return connection;
		}

		public void Dispose()
		{
			config?.Dispose();
		}
	}
}