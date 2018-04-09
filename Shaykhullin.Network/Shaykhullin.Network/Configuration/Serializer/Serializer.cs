using System;
using System.Text;

namespace Network.Core
{
  internal class Serializer : ISerializer
  {
    public byte[] Serialize(object @object)
    {
      return Encoding.UTF8.GetBytes(@object.ToString());
    }

    public object Deserialize(byte[] data, Type type)
    {
      return Encoding.UTF8.GetString(data);
    }
  }
}
