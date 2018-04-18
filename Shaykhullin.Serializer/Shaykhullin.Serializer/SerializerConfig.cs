using System;
using System.Collections;

using Shaykhullin.Activator;
using Shaykhullin.Serializer.Core;
using Shaykhullin.DependencyInjection;

namespace Shaykhullin.Serializer
{
	public class SerializerConfig : ISerializerConfig
	{
		private readonly IContainerConfig root;
		private readonly IContainerConfig scope;
		private readonly ConverterContainer converterContainer;

		public SerializerConfig()
		{
			root = new ContainerConfig();

			root.Register<IActivator>()
				.ImplementedBy(c => new ActivatorConfig().Create())
				.As<Singleton>();
			
			using (var scope = root.CreateScope())
			{
				converterContainer = new ConverterContainer(scope);

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
				Match<DateTime>().With<DateTimeConverter>();
				Match<IList>().With<IListConverter>();
				Match<ICollection>().With<CollectionConverter>();
				Match<IEnumerable>().With<IEnumerableConverter>();

				this.scope = scope;
			}
		}

		internal SerializerConfig(SerializerConfig parent)
		{
			scope = parent.scope.CreateScope();
			converterContainer = new ConverterContainer(scope, parent.converterContainer);
		}

		public IUseBuilder<TData> Match<TData>()
		{
			converterContainer.RegisterTypeWithAlias(typeof(TData));
			return new UseBuilder<TData>(scope ?? root, converterContainer);
		}
		
		public ISerializerConfig CreateScope() => new SerializerConfig(this);

		public ISerializer Create() => new Core.Serializer(scope, converterContainer);

		public void Dispose()
		{
			root?.Dispose();
			scope?.Dispose();
		}
	}
}
