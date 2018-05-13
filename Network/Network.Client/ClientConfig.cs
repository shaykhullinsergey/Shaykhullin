using System;
using Network.Core;

namespace Network
{
	public readonly struct ClientConfig : IDisposable
	{
		private readonly Configuration configuration;

		public ClientConfig(int test = 0)
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
		
		public Client Create()
		{
			return new Client(configuration);
		}

		public void Dispose()
		{
		}
	}
}