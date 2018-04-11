using Shaykhullin.Serializer.Core;

namespace Shaykhullin.Serializer
{
	public interface ISerializerConfig
	{
		IUseBuilder<TData> Match<TData>();
		ISerializer Create();
	}
}
