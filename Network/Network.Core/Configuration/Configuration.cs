using System;
using System.Collections.Generic;

namespace Network.Core
{
	internal class Configuration
	{
		private readonly Dictionary<Type, ChannelConfiguration> channels;

		public Configuration()
		{
			channels = new Dictionary<Type, ChannelConfiguration>();
		}
		
		public ChannelConfiguration EnsureChannel(Type channelType)
		{
			if (!channels.TryGetValue(channelType, out var channelConfiguration))
			{
				channelConfiguration = new ChannelConfiguration();
				channels.Add(channelType, channelConfiguration);
			}

			return channelConfiguration;
		}

		public ChannelConfiguration Channel(Type channelType)
		{
			return channels[channelType];
		}
	}
}