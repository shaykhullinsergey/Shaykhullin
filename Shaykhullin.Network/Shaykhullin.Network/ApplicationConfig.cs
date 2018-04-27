using System;
using Shaykhullin.Network.Core;

using Shaykhullin.Serializer;
using Shaykhullin.DependencyInjection;
using Shaykhullin.DependencyInjection.Core;

namespace Shaykhullin.Network
{
	public abstract class ApplicationConfig<TNode> : IConfig<TNode>
		where TNode : IApplication
	{
		private readonly IContainerConfig config;

		protected ApplicationConfig()
		{
			var rootConfig = new ContainerConfig();
			
			rootConfig.Register<ISerializerConfig>()
				.ImplementedBy(c => new SerializerConfig())
				.As<Singleton>();
				
			rootConfig.Register<ISerializer>()
				.ImplementedBy(c => c.Resolve<ISerializerConfig>().Create())
				.As<Singleton>();
			
			rootConfig.Register<ICompression>()
				.ImplementedBy<Compression>()
				.As<Singleton>();
			
			rootConfig.Register<IEncryption>()
				.ImplementedBy<Encryption>()
				.As<Singleton>();

			rootConfig.Register<IConnection>()
				.ImplementedBy<Connection>()
				.As<Transient>();

			rootConfig.Register<CommandCollection>()
				.ImplementedBy<CommandCollection>()
				.As<Singleton>();

			rootConfig.Register<HandlerCollection>()
				.ImplementedBy<HandlerCollection>()
				.As<Singleton>();

			rootConfig.Register<ICommandHolder>()
				.ImplementedBy<CommandHolder>()
				.As<Singleton>();

			rootConfig.Register<Disconnect>();
			rootConfig.Register<Connect>();
			rootConfig.Register<Error>();

			config = rootConfig.CreateScope();

			config.Register<IContainerConfig>()
				.ImplementedBy(c => config)
				.As<Singleton>();
		}
		
		public ICompressionBuilder UseSerializer<TSerializer>(TSerializer serializer = default) 
			where TSerializer : ISerializer
		{
			return new SerializerBuilder(config).UseSerializer(serializer);
		}
		
		public IEncryptionBuilder UseCompression<TCompression>()
			where TCompression : ICompression
		{
			return new SerializerBuilder(config).UseCompression<TCompression>();
		}
		
		public void UseEncryption<TEncryption>()
			where TEncryption : IEncryption
		{
			new SerializerBuilder(config).UseEncryption<TEncryption>();
		}

		public IImplementedByBuilder<TRegistry> Register<TRegistry>() 
		{
			return config.Register<TRegistry>();
		}

		public IImplementedByBuilder<object> Register(Type registry)
		{
			return config.Register(registry);
		}

		public IImplementedByBuilder<TRegistry> Register<TRegistry>(Type registry)
		{
			return config.Register<TRegistry>(registry);
		}

		public ICommandBuilder<TData> On<TData>()
		{
			return new CommandBuilder<TData>(config);
		}

		protected IContainerConfig Configure(string host, int port)
		{
			config.Register<IConfiguration>()
				.ImplementedBy(c => new Configuration(host, port))
				.As<Singleton>();

			return config;
		}
		
		public abstract TNode Create(string host, int port);
	}
}