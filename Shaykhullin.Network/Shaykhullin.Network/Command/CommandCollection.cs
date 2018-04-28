using Shaykhullin.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Shaykhullin.Network.Core
{
	public class CommandCollection
	{
		private static readonly Dictionary<Type, Type> GenericArgumentsCache = new Dictionary<Type, Type>();
		
		private readonly IContainerConfig config;
		private readonly Dictionary<int, Type> commands = new Dictionary<int, Type>();

		public CommandCollection(IContainerConfig config)
		{
			this.config = config;
		}

		public void Add<TData, TCommand>()
			where TCommand : ICommand<TData>
		{
			var command = typeof(TCommand);
			
			if (!commands.ContainsValue(command))
			{
				config.Register<TCommand>();
				commands.Add(GetHash(command.Name), command);
			}
		}
		
		public Type GetCommand(int id)
		{
			return commands[id];
		}

		public int GetCommand(Type command)
		{
			foreach (var item in commands)
			{
				if (item.Value == command)
				{
					return item.Key;
				}
			}

			var hash = GetHash(command.Name);
			commands.Add(hash, command);
			config.Register<object>(command);
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