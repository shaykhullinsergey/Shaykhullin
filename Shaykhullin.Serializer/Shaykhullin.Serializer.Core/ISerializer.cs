using System;
using System.IO;

namespace Shaykhullin.Serializer
{
	public interface ISerializer
	{
		void Serialize<TData>(Stream stream, TData data);
		TData Deserialize<TData>(Stream stream);
		void Serialize(Stream stream, object data, Type dataTypeOverride = null);
		object Deserialize(Stream stream, Type type);
	}
}
