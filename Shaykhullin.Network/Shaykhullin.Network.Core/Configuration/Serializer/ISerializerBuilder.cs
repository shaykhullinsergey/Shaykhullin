namespace Network.Core
{
	public interface ISerializerBuilder : ICompressionBuilder
	{
		ICompressionBuilder UseSerializer<TSerializer>()
			where TSerializer : ISerializer;
	}
}