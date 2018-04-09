using Shaykhullin.DependencyInjection;

namespace Shaykhullin.Network.Core
{
	internal class EncryptionBuilder : IEncryptionBuilder
	{
		private readonly IContainerConfig config;

		public EncryptionBuilder(IContainerConfig config)
		{
			this.config = config;
		}

		public void UseEncryption<TEncryption>() 
			where TEncryption : IEncryption
		{
			config.Register<IEncryption>()
				.ImplementedBy<TEncryption>()
				.As<Singleton>();
		}
	}
}
