using Shaykhullin.DependencyInjection;

namespace Network.Core
{
	internal class ConfigurationBuilder : IConfigurationBuilder
	{
		private readonly IContainerConfig config;

		public ConfigurationBuilder(IContainerConfig config)
		{
			this.config = config;
		}
		
		public ICompressionBuilder UseSerializer<TSerializer>() 
			where TSerializer : ISerializer
		{
			return new SerializerBuilder(config)
				.UseSerializer<TSerializer>();
		}
		
		public IEncryptionBuilder UseCompression<TCompression>() 
			where TCompression : ICompression
		{
			return new SerializerBuilder(config)
				.UseCompression<TCompression>();
		}

		public void UseEncryption<TEncryption>()
			where TEncryption : IEncryption
		{
			new SerializerBuilder(config)
				.UseEncryption<TEncryption>();
		}
	}
}
