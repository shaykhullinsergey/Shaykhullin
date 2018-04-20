using System;
using System.Collections;
using Shaykhullin.Activator;
using Shaykhullin.Serializer.Core;
using Shaykhullin.DependencyInjection;

namespace Shaykhullin.Serializer
{
	public class SerializerConfig : ISerializerConfig
	{
		private bool disposed;
		private readonly IContainerConfig config;
		private readonly ConverterContainer converterContainer;

		public SerializerConfig()
		{
			config = new ContainerConfig();

			config.Register<IActivator>()
				.ImplementedBy(c => new ActivatorConfig().Create())
				.As<Singleton>();

			var innerScope = config.CreateScope();
			converterContainer = new ConverterContainer(innerScope);

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
			Match<DateTime>().With<DateTimeConverter>();
			Match<Array>().With<ArrayConverter>();
			Match<IList>().With<IListConverter>();
			Match<ICollection>().With<CollectionConverter>();
			Match<IEnumerable>().With<IEnumerableConverter>();

			config = innerScope;
			converterContainer = new ConverterContainer(config, converterContainer);
		}

		internal SerializerConfig(SerializerConfig parent)
		{
			config = parent.config.CreateScope();
			converterContainer = new ConverterContainer(config, parent.converterContainer);
		}

		public IUseBuilder<TData> Match<TData>()
		{
			if (disposed)
			{
				throw new ObjectDisposedException(nameof(SerializerConfig));
			}

			converterContainer.RegisterTypeWithAlias(typeof(TData));
			return new UseBuilder<TData>(config, converterContainer);
		}

		public ISerializerConfig CreateScope()
		{
			if (disposed)
			{
				throw new ObjectDisposedException(nameof(SerializerConfig));
			}

			return new SerializerConfig(this);
		}

		public ISerializer Create()
		{
			if (disposed)
			{
				throw new ObjectDisposedException(nameof(SerializerConfig));
			}

			var scope = config.CreateScope();
			var converter = new ConverterContainer(scope, converterContainer);

			using (var container = scope.Create())
			{
				var activator = container.Resolve<IActivator>();
				var serializer = new Core.Serializer(activator, converter);
				
				scope.Register<ISerializer>()
					.ImplementedBy(c => serializer)
					.As<Singleton>();
				
				return serializer;
			}
		}

		public void Dispose()
		{
			if (!disposed)
			{
				disposed = true;
				config?.Dispose();
			}
		}
	}
}