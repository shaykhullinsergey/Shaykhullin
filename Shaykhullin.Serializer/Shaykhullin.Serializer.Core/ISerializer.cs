using System;
using Shaykhullin.Stream;

namespace Shaykhullin.Serializer
{
	public interface ISerializer : IDisposable
	{
		void Serialize<TData>(ValueStream stream, TData data);
		TData Deserialize<TData>(ValueStream stream);
		void Serialize(ValueStream stream, object data, Type dataTypeOverride = null);
		object Deserialize(ValueStream stream, Type type);
	}
}
