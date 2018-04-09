using Shaykhullin.DependencyInjection;

namespace Network.Core
{
	internal class CompressionBuilder : ICompressionBuilder
	{
		private readonly IContainerConfig config;

		public CompressionBuilder(IContainerConfig config)
		{
			this.config = config;
		}

		public IEncryptionBuilder UseCompression<TCompression>() 
			where TCompression : ICompression
		{
			config.Register<ICompression>()
				.ImplementedBy<TCompression>()
				.As<Singleton>();
			
			return new EncryptionBuilder(config);
		}

		public void UseEncryption<TEncryption>() 
			where TEncryption : IEncryption
		{
			new EncryptionBuilder(config)
				.UseEncryption<TEncryption>();
		}
	}
}
