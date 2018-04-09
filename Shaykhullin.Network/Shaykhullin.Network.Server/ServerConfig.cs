using Shaykhullin.Network.Core;

namespace Shaykhullin.Network
{
	public sealed class ServerConfig : Config<IServer>
	{
		public override IServer Create(string host, int port)
		{
			var config = Configure(host, port);
			return new Server(config);
		}
	}
}