using System;
using Shaykhullin.Serializer.Core;

namespace Shaykhullin.Serializer
{
	public interface ISerializerConfig : IDisposable
	{
		ISerializerConfig CreateScope();
		IUseBuilder<TData> Match<TData>();
		ISerializer Create();
	}
}
