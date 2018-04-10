using System;
using System.Collections.Generic;

using Shaykhullin.Activator;
using Shaykhullin.DependencyInjection;
using Shaykhullin.Serializer.Core;

namespace Shaykhullin.Serializer
{
	public class ConverterCollection
	{
		private readonly Dictionary<Type, Type> converters;

		public ConverterCollection()
		{
			converters = new Dictionary<Type, Type>();
		}

		public void Add(Type type, Type converterType, IContainerConfig config)
		{
			if (converters.ContainsKey(type))
			{
				converters[type] = converterType;
			}
			else
			{
				converters.Add(type, converterType);
			}

			config.Register(converterType)
				.As<Singleton>();
		}

		public bool TryGetValue(Type type, out Type converterType)
		{
			return converters.TryGetValue(type, out converterType);
		}
	}

	public class SerializerConfig : ISerializerConfig
	{
		private ISerializer serializer;
		private readonly IContainerConfig scope;
		private readonly IContainerConfig rootConfig;
		private readonly ConverterCollection converters;

		public SerializerConfig()
		{
			rootConfig = new ContainerConfig();
			converters = new ConverterCollection();

			rootConfig.Register<IActivator>()
				.ImplementedBy(c => new ActivatorConfig().Create())
				.As<Singleton>();

			new UseBuilder<int>(rootConfig, converters).Use<Int32Converter>();
			new UseBuilder<int>(rootConfig, converters).Use<Int32Converter>();

			scope = rootConfig.Scope();
		}

		public IUseBuilder<TData> When<TData>()
		{
			return new UseBuilder<TData>(scope, converters);
		}

		public ISerializer Create()
		{
			return serializer ?? (serializer = new Serializer(scope.Container, converters));
		}
	}
}
