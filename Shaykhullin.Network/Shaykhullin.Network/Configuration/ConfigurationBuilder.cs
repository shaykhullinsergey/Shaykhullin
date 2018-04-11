using Shaykhullin.DependencyInjection;
using Shaykhullin.Serializer;

namespace Shaykhullin.Network.Core
{
	internal class ConfigurationBuilder : IConfigurationBuilder
	{
		private readonly IContainerConfig config;

		public ConfigurationBuilder(IContainerConfig config)
		{
			this.config = config;
		}
		
		public ICompressionBuilder UseSerializer<TSerializer>(TSerializer serializer = default) 
			where TSerializer : ISerializer
		{
			return new SerializerBuilder(config)
				.UseSerializer<TSerializer>(serializer);
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
