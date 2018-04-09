using System.IO;

namespace Shaykhullin.Serializer.Core
{
	public interface IConverter<TData> : IConverter
	{
		void Serialize(Stream stream, TData data);
		TData Deserialize(Stream stream);
	}

	public interface IConverter
	{
		void SerializeObject(Stream stream, object obj);
		object DeserializeObject(Stream stream);
	}
}