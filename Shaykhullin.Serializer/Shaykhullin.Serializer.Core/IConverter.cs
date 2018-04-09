using System.IO;
using Shaykhullin.Serializer.Core;

namespace Shaykhullin.Serializer
{
	public interface IConverter<TData> : IConverter
	{
		void Serialize(Stream stream, TData data);
		TData Deserialize(Stream stream);
	}
}


namespace Shaykhullin.Serializer.Core
{
	public interface IConverter
	{
		void SerializeObject(Stream stream, object obj);
		object DeserializeObject(Stream stream);
	}
}