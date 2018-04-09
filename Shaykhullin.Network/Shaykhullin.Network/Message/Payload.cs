using System;

namespace Network.Core
{
  internal class Payload : IPayload
	{
    public Type Event { get; set; }
    public object Data { get; set; }
  }
}
