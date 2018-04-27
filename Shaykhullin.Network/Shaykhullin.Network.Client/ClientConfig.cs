using Shaykhullin.Network.Core;

namespace Shaykhullin.Network
{
	public sealed class ClientConfig : ApplicationConfig<IClient>
	{
		public override IClient Create(string host, int port)
		{
			var config = Configure(host, port);
			return new ClientApplication(config);
		}
	}
}