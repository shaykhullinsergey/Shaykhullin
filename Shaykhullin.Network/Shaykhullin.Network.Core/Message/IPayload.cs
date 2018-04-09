using System;

namespace Shaykhullin.Network.Core
{
	public interface IPayload
	{
		object Data { get; }
		Type Event { get; }
	}
}
