using Shaykhullin.Network.Core;

namespace Shaykhullin.Network
{
	public sealed class ServerConfig : ApplicationConfig<IServer>
	{
		public override IServer Create(string host, int port)
		{
			var config = Configure(host, port);
			return new ServerApplication(config);
		}
	}
}