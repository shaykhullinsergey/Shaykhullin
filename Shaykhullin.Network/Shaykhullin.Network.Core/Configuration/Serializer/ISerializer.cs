using System;

namespace Network.Core
{
	public interface ISerializer
	{
		byte[] Serialize(object @object);

		object Deserialize(byte[] data, Type type);
	}
}
