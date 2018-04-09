using Shaykhullin.Network.Core;

namespace Shaykhullin.Network
{
	public sealed class ClientConfig : Config<IClient>
	{
		public override IClient Create(string host, int port)
		{
			var config = base.Configure(host, port);
			return new Client(config);
		}
	}
}