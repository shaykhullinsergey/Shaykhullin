using System;

namespace Shaykhullin.Network.Core
{
  internal class Payload<TData> : IPayload<TData>
	{
    public Type CommandType { get; set; }
    public TData Data { get; set; }
  }
}
