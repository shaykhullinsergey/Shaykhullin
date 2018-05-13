using System;
using System.Collections.Generic;

namespace Network.Core
{
	internal class CommandConfiguration
	{
		private readonly Dictionary<Type, HandlerConfiguration> commands;

		public CommandConfiguration()
		{
			commands = new Dictionary<Type, HandlerConfiguration>();
		}
		
		public HandlerConfiguration EnsureCommand(Type commandType)
		{
			if (!commands.TryGetValue(commandType, out var commandConfiguration))
			{
				commandConfiguration = new HandlerConfiguration();
				commands.Add(commandType, commandConfiguration);
			}

			return commandConfiguration;
		}
		
		public HandlerConfiguration Command(Type commandType)
		{
			return commands[commandType];
		}
	}
}