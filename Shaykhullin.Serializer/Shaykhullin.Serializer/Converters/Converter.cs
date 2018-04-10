using System;
using System.IO;
using Shaykhullin.Serializer.Core;

namespace Shaykhullin.Serializer
{
	public abstract class Converter<TData> : IConverter<TData>
	{
		public abstract TData Deserialize(Stream stream);
		public abstract void Serialize(Stream stream, TData data);

		public virtual object DeserializeObject(Stream stream, Type type)
		{
			return Deserialize(stream);
		}
		public virtual void SerializeObject(Stream stream, object obj)
		{
			Serialize(stream, (TData)obj);
		}
	}
}
