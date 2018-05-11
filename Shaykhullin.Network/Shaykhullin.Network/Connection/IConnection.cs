using System;
using Shaykhullin.Network.Core;

namespace Shaykhullin.Network
{
	public interface IConnection : IDisposable
	{
		SendBuilder<TData> Send<TData>(TData data);
	}
}
