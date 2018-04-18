using System;
using Shaykhullin.Network.Core;

using Shaykhullin.Serializer;
using Shaykhullin.DependencyInjection;
using Shaykhullin.DependencyInjection.Core;

namespace Shaykhullin.Network
{
	public abstract class Config<TNode> : IConfig<TNode>
		where TNode : INode
	{
		private readonly IContainerConfig rootConfig;
		private readonly IContainerConfig config;

		protected Config()
		{
			rootConfig = new ContainerConfig();
			
			var serializerConfig = new SerializerConfig();

			rootConfig.Register<ISerializerConfig>()
				.ImplementedBy(c => serializerConfig)
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

			rootConfig.Register<EventCollection>()
				.ImplementedBy<EventCollection>()
				.As<Singleton>();

			rootConfig.Register<HandlerCollection>()
				.ImplementedBy<HandlerCollection>()
				.As<Singleton>();

			rootConfig.Register<IEventHolder>()
				.ImplementedBy<EventHolder>()
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

		public IImplementedByBuilder<TRegister> Register<TRegister>() 
		{
			return config.Register<TRegister>();
		}

		public IImplementedByBuilder<object> Register(Type register)
		{
			return config.Register(register);
		}

		public IImplementedByBuilder<TRegister> Register<TRegister>(Type register)
		{
			return config.Register<TRegister>(register);
		}

		public IEventBuilder<TData> Match<TData>()
		{
			return new EventBuilder<TData>(config);
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