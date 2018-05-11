using Shaykhullin.Serializer;

namespace Shaykhullin.Network.Core
{
	public interface ISerializerBuilder : ICompressionBuilder
	{
		ICompressionBuilder UseSerializer<TSerializer>(TSerializer serializer = default)
			where TSerializer : ISerializer;
	}
}