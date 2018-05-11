using System;
using Shaykhullin.Serializer.Core;
using Shaykhullin.Stream;

namespace Shaykhullin.Serializer.Core
{
	public abstract class Converter<TData> : IConverter<TData>
	{
		public abstract TData Deserialize(ValueStream stream);
		public abstract void Serialize(ValueStream stream, TData data);

		public virtual object DeserializeObject(ValueStream stream, Type type)
		{
			return Deserialize(stream);
		}
		public virtual void SerializeObject(ValueStream stream, object obj)
		{
			Serialize(stream, (TData)obj);
		}
	}
}
