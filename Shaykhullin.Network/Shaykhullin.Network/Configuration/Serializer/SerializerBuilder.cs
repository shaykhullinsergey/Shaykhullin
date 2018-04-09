using Shaykhullin.DependencyInjection;
using Shaykhullin.Serializer;

namespace Network.Core
{
	internal class SerializerBuilder : ISerializerBuilder
	{
		private readonly IContainerConfig config;

		public SerializerBuilder(IContainerConfig config)
		{
			this.config = config;
		}

		public ICompressionBuilder UseSerializer<TSerializer>() 
			where TSerializer : ISerializer
		{
			config.Register<ISerializer>()
				.ImplementedBy<TSerializer>()
				.As<Singleton>();
			
			return new CompressionBuilder(config);
		}
		
		public IEncryptionBuilder UseCompression<TCompression>() 
			where TCompression : ICompression
		{
			return new CompressionBuilder(config)
				.UseCompression<TCompression>();
		}
		
		public void UseEncryption<TEncryption>() 
			where TEncryption : IEncryption
		{
			new CompressionBuilder(config)
				.UseEncryption<TEncryption>();
		}
	}
}