using System;
using System.Collections.Generic;
using Shaykhullin.DependencyInjection;

namespace Shaykhullin.Network.Core
{
	internal class CommandCollection
	{
		private static readonly Dictionary<Type, Type> GenericArgumentsCache = new Dictionary<Type, Type>();
		
		private readonly IContainerConfig config;
		private readonly List<CommandDto> commands = new List<CommandDto>();

		public CommandCollection(IContainerConfig config)
		{
			this.config = config;
		}

		public void Add<TData, TCommand>()
			where TCommand : ICommand<TData>
		{
			var command = typeof(TCommand);

			for (var i = 0; i < commands.Count; i++)
			{
				if (commands[i].CommandType == command)
				{
					return;
				}
			}
			
			config.Register<TCommand>();
			commands.Add(new CommandDto(GetHash(command.Name), command));
		}
		
		public Type GetCommand(int commandId)
		{
			for (var i = 0; i < commands.Count; i++)
			{
				if (commands[i].CommandId == commandId)
				{
					return commands[i].CommandType;
				}
			}
			
			throw new InvalidOperationException(nameof(GetCommand));
		}

		public int GetCommand(Type commandType)
		{
			foreach (var command in commands)
			{
				if (command.CommandType == commandType)
				{
					return command.CommandId;
				}
			}

			var hash = GetHash(commandType.Name);
			commands.Add(new CommandDto(hash, commandType));
			config.Register<object>(commandType);
			return hash;
		}
		
		private static unsafe int GetHash(string name)
		{
			fixed (char* str = name)
			{
				var chPtr = str;
				var num = 352654597;
				var num2 = num;
				var numPtr = (int*)chPtr;
				for (var i = name.Length; i > 0; i -= 4)
				{
					num = (num << 5) + num + (num >> 27) ^ numPtr[0];
					if (i <= 2)
					{
						break;
					}
					num2 = (num2 << 5) + num2 + (num2 >> 27) ^ numPtr[1];
					numPtr += 2;
				}
				return num + num2 * 1566083941;
			}
		}

		public Type GetGenericArgument(Type commandType)
		{
			if (!GenericArgumentsCache.TryGetValue(commandType, out var genericArgument))
			{
				genericArgument = commandType.GetInterfaces()[0].GetGenericArguments()[0];
				GenericArgumentsCache.Add(commandType, genericArgument);
			}

			return genericArgument;
		}
	}
}