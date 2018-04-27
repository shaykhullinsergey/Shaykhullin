using System;

namespace Shaykhullin.Network.Core
{
  internal class Payload : IPayload
	{
    public Type CommandType { get; set; }
    public object Data { get; set; }
  }
}
