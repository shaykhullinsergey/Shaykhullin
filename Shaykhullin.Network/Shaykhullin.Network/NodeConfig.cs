using System;
using Network.Core;
using Shaykhullin.DependencyInjection;
using Shaykhullin.DependencyInjection.Core;

namespace Network
{
	public abstract class NodeConfig<TNode> : IConfig<TNode>
		where TNode : INode
	{
		private readonly IContainerConfig rootConfig = new ContainerConfig();
		protected readonly IContainerConfig Config;

		protected NodeConfig()
		{
			rootConfig.Register<ISerializer>()
				.ImplementedBy<Serializer>()
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
			rootConfig.Register<Error>();

			Config = rootConfig.Scope();

			Config.Register<IContainerConfig>()
				.ImplementedBy(c => Config)
				.As<Singleton>();
		}
		
		public ICompressionBuilder UseSerializer<TSerializer>() 
			where TSerializer : ISerializer
		{
			return new SerializerBuilder(Config)
				.UseSerializer<TSerializer>();
		}
		public IEncryptionBuilder UseCompression<TCompression>()
			where TCompression : ICompression
		{
			return new SerializerBuilder(Config)
				.UseCompression<TCompression>();
		}
		public void UseEncryption<TEncryption>()
			where TEncryption : IEncryption
		{
			new SerializerBuilder(Config)
				.UseEncryption<TEncryption>();
		}

		public IImplementedByBuilder<TRegister> Register<TRegister>() 
			where TRegister : class
		{
			return Config.Register<TRegister>();
		}

		public IImplementedByBuilder<object> Register(Type register)
		{
			return Config.Register(register);
		}

		public IHandlerBuilder<TEvent> When<TEvent>() 
			where TEvent : class, IEvent<object>
		{
			return new EventBuilder(Config)
				.When<TEvent>();
		}
		public abstract TNode Create(string host, int port);

		protected void Configure(string host, int port)
		{
			Config.Register<IConfiguration>()
				.ImplementedBy(c => new Configuration(host, port))
				.As<Singleton>();
		}
	}
}