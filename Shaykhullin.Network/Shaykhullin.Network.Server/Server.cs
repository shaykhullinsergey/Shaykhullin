using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Shaykhullin.DependencyInjection;

namespace Shaykhullin.Network.Core
{
	internal class Server : IServer
	{
		private readonly IContainerConfig config;

		public Server(IContainerConfig config)
		{
			this.config = config;
		}

		public async Task Run()
		{
			var rootContainer = config.Create();

			var configuration = rootContainer.Resolve<IConfiguration>();

			var tcpListener = new TcpListener(new IPEndPoint(IPAddress.Parse(configuration.Host), configuration.Port));
			tcpListener.Start();

			while (true)
			{
				var tcpClient = await tcpListener.AcceptTcpClientAsync().ConfigureAwait(false);

				tcpClient.NoDelay = true;
				tcpClient.ReceiveTimeout = 0;
				tcpClient.SendTimeout = 0;
				tcpClient.ReceiveBufferSize = 256;
				tcpClient.SendBufferSize = 256;

				var scope = config.CreateScope();
				
				scope.Register<IContainerConfig>()
					.ImplementedBy(c => scope)
					.As<Singleton>();

				var container = scope.Create();

				scope.Register<IContainer>()
					.ImplementedBy(c => container)
					.As<Singleton>();

				scope.Register<TcpClient>()
					.ImplementedBy(c => tcpClient)
					.As<Singleton>();

				var connection = container.Resolve<IConnection>();

				scope.Register<IConnection>()
					.ImplementedBy(c => connection)
					.As<Singleton>();
			}
		}
	}
}