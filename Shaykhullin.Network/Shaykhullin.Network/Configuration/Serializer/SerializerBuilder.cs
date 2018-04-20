using Shaykhullin.DependencyInjection;
using Shaykhullin.Serializer;

namespace Shaykhullin.Network.Core
{
	internal class SerializerBuilder : ISerializerBuilder
	{
		private readonly IContainerConfig config;

		public SerializerBuilder(IContainerConfig config)
		{
			this.config = config;
		}

		public ICompressionBuilder UseSerializer<TSerializer>(TSerializer serializer = default) 
			where TSerializer : ISerializer
		{
			var registry = config.Register<ISerializer>();

			if(serializer == null)
			{
				registry.ImplementedBy<TSerializer>().As<Singleton>();
			}
			else
			{
				registry.ImplementedBy(c => serializer).As<Singleton>();
			}
			
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