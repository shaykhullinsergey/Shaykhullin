namespace Shaykhullin.Network.Core
{
	public interface ICompressionBuilder : IEncryptionBuilder
	{
		IEncryptionBuilder UseCompression<TCompression>()
			where TCompression : ICompression;
	}
}
