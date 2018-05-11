using System;

namespace Shaykhullin.Network.Core
{
	public interface IPayload<TData>
	{
		TData Data { get; }
		Type CommandType { get; }
	}
}
