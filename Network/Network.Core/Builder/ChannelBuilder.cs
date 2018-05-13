using System;

namespace Network.Core
{
	public readonly struct ChannelBuilder<TChannel> : IDisposable
		where TChannel : IChannel
	{
		private readonly ChannelConfiguration configuration;
		
		internal ChannelBuilder(ChannelConfiguration configuration)
		{
			this.configuration = configuration;
		}

		public ChannelBuilder<TChannel> Channel<TChannel>()
			where TChannel : IChannel
		{
			var channel = configuration.EnsureChannel(typeof(TChannel));
			return new ChannelBuilder<TChannel>(channel);
		}
		
		public CommandBuilder<TData> When<TData>()
		{
			var when = configuration.EnsureDataType(typeof(TData));
			return new CommandBuilder<TData>(when);
		}

		public void Dispose()
		{
		}
	}
}