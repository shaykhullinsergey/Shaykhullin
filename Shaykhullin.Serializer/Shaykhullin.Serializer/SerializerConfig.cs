
using Shaykhullin.Activator;
using Shaykhullin.DependencyInjection;
using Shaykhullin.Serializer.Core;
using System;

namespace Shaykhullin.Serializer
{
	public class SerializerConfig : ISerializerConfig
	{
		private ISerializer serializer;
		private readonly IContainerConfig scope;
		private readonly IContainerConfig rootConfig;
		private readonly ConverterCollection converters;

		public SerializerConfig()
		{
			rootConfig = new ContainerConfig();
			scope = rootConfig.Scope();
			converters = new ConverterCollection(scope.Container);

			rootConfig.Register<IActivator>()
				.ImplementedBy(c => new ActivatorConfig().Create())
				.As<Singleton>();

			rootConfig.Register<ConverterCollection>()
				.ImplementedBy(c => converters)
				.As<Singleton>();

			scope.Register<IContainer>()
				.ImplementedBy(c => c)
				.As<Singleton>();

			new UseBuilder<byte>(rootConfig, converters).Use<ByteConverter>();
			new UseBuilder<sbyte>(rootConfig, converters).Use<SByteConverter>();
			new UseBuilder<short>(rootConfig, converters).Use<ShortConverter>();
			new UseBuilder<ushort>(rootConfig, converters).Use<UShortConverter>();
			new UseBuilder<int>(rootConfig, converters).Use<Int32Converter>();
			new UseBuilder<uint>(rootConfig, converters).Use<UInt32Converter>();
			new UseBuilder<long>(rootConfig, converters).Use<Int64Converter>();
			new UseBuilder<ulong>(rootConfig, converters).Use<UInt64Converter>();
			new UseBuilder<string>(rootConfig, converters).Use<StringConverter>();
			new UseBuilder<Array>(rootConfig, converters).Use<ArrayConverter>();
		}

		public IUseBuilder<TData> When<TData>()
		{
			return new UseBuilder<TData>(scope, converters);
		}

		public ISerializer Create()
		{
			return serializer ?? (serializer = new Serializer(scope, converters));
		}
	}
}
