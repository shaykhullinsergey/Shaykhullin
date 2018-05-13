using System;
using Network.Core;

namespace Network
{
	public readonly struct ServerConfig : IDisposable
	{
		private readonly Configuration configuration;

		public ServerConfig(int test = 0)
		{
			configuration = new Configuration();
		}
		
		public ChannelBuilder<TChannel> Channel<TChannel>()
			where TChannel : IChannel
		{
			var channel = configuration.EnsureChannel(typeof(TChannel));
			
			return new ChannelBuilder<TChannel>(channel);
		}
		
		public CommandBuilder<TData> When<TData>()
		{
			var channel = configuration.EnsureChannel(typeof(DefaultChannel));
			
			return new ChannelBuilder<DefaultChannel>(channel)
				.When<TData>();
		}
		
		public Server Create()
		{
			return new Server(configuration);
		}

		public void Dispose()
		{
		}
	}
}