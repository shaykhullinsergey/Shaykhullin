using System;
using Shaykhullin.Network.Core;

namespace Shaykhullin.Network
{
	public interface IConnection : IDisposable
	{
		ISendBuilder<TData> Send<TData>(TData data);
	}
}
