using Shaykhullin.Serializer.Core;

namespace Shaykhullin.Serializer
{
	public interface ISerializerConfig
	{
		IUseBuilder<TData> When<TData>();
		ISerializer Create();
	}
}
