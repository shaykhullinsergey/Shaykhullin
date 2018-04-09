using System;

namespace Network.Core
{
	public interface IPayload
	{
		object Data { get; }
		Type Event { get; }
	}
}
