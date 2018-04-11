using System;
using System.Collections;

using Shaykhullin.Activator;
using Shaykhullin.Serializer.Core;
using Shaykhullin.DependencyInjection;

namespace Shaykhullin.Serializer
{
	public class SerializerConfig : ISerializerConfig
	{
		private ISerializer serializer;
		private readonly IContainerConfig scope;
		private readonly IContainerConfig rootConfig;
		private readonly Configuration configuration;

		public SerializerConfig()
		{
			rootConfig = new ContainerConfig();

			using (var scope = rootConfig.Scope())
			{
				configuration = new Configuration(scope.Container);

				rootConfig.Register<IActivator>()
					.ImplementedBy(c => new ActivatorConfig().Create())
					.As<Singleton>();

				rootConfig.Register<Configuration>()
					.ImplementedBy(c => configuration)
					.As<Singleton>();

				scope.Register<IContainer>()
					.ImplementedBy(c => c)
					.As<Singleton>();

				Match<byte>().With<ByteConverter>();
				Match<sbyte>().With<SByteConverter>();
				Match<short>().With<ShortConverter>();
				Match<ushort>().With<UShortConverter>();
				Match<int>().With<Int32Converter>();
				Match<uint>().With<UInt32Converter>();
				Match<long>().With<Int64Converter>();
				Match<ulong>().With<UInt64Converter>();
				Match<string>().With<StringConverter>();
				Match<bool>().With<BoolConverter>();
				Match<float>().With<SingleConverter>();
				Match<double>().With<DoubleConverter>();
				Match<Array>().With<ArrayConverter>();
				Match<IList>().With<IListConverter>();
				Match<ICollection>().With<CollectionConverter>();
				Match<IEnumerable>().With<IEnumerableConverter>();

				this.scope = scope;
			}
		}

		public IUseBuilder<TData> Match<TData>()
		{
			configuration.RegisterTypeWithAlias(typeof(TData));
			return new UseBuilder<TData>(scope ?? rootConfig, configuration);
		}

		public ISerializer Create()
		{
			return serializer ?? (serializer = new Core.Serializer(scope, configuration));
		}
	}
}
