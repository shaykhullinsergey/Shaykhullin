using System;

namespace Shaykhullin.Network.Core
{
	internal class CommandDto
	{
		public int CommandId { get; }
		public Type CommandType { get; }

		public CommandDto(int commandId, Type commandType)
		{
			CommandId = commandId;
			CommandType = commandType;
		}
	}
}