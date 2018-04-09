using System;
using System.Text;
using Network.Core;
using Newtonsoft.Json;

namespace Network.Extensions
{
	public class JsonSerializer : ISerializer
	{
		public byte[] Serialize(object @object)
		{
			return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@object));
		}

		public object Deserialize(byte[] data, Type type)
		{
			return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data), type);
		}
	}
}