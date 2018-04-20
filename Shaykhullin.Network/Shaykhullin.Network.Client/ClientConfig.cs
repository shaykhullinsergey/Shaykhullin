using Shaykhullin.Network.Core;

namespace Shaykhullin.Network
{
	public sealed class ClientConfig : NodeConfig<IClient>
	{
		public override IClient Create(string host, int port)
		{
			var config = base.Configure(host, port);
			return new Client(config);
		}
	}
}