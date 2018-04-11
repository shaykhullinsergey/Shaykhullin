using Shaykhullin.Serializer.Core;

namespace Shaykhullin.Serializer
{
	public interface ISerializerConfig
	{
		void UseTypeAliasing();
		IUseBuilder<TData> Match<TData>();
		ISerializer Create();
	}
}
