using Network.Core;

namespace Network
{
	public sealed class ClientConfig : NodeConfig<IClient>
	{
		public override IClient Create(string host, int port)
		{
			base.Configure(host, port);
			return new Client(Config);
		}
	}
}