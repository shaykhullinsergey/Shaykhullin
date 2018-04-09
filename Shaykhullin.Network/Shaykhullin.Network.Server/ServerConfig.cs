using Network.Core;

namespace Network
{
	public sealed class ServerConfig : NodeConfig<IServer>
	{
		public override IServer Create(string host, int port)
		{
			base.Configure(host, port);
			return new Server(Config);
		}
	}
}