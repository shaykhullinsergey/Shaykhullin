using System;
using Shaykhullin.Stream;

namespace Shaykhullin.Serializer.Core
{
	public interface IConverter<TData> : IConverter
	{
		void Serialize(ValueStream stream, TData data);
		TData Deserialize(ValueStream stream);
	}

	public interface IConverter
	{
		void SerializeObject(ValueStream stream, object obj);
		object DeserializeObject(ValueStream stream, Type type);
	}
}