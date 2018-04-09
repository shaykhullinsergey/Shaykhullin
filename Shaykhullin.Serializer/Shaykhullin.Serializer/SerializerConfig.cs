using System;
using System.Collections.Generic;

using Shaykhullin.Activator;
using Shaykhullin.Serializer.Core;

namespace Shaykhullin.Serializer
{
	public class SerializerConfig : ISerializerConfig
	{
		private ISerializer serializer;
		private readonly IActivator activator;
		private readonly Dictionary<Type, IConverter> converters;

		public SerializerConfig()
		{
			activator = new ActivatorConfig().Create();
			converters = new Dictionary<Type, IConverter>();

			When<int>().Use<Int32Converter>();
			When<byte>().Use<ByteConverter>();
		}

		public IUseBuilder<TData> When<TData>()
		{
			return new UseBuilder<TData>(activator, converters);
		}

		public ISerializer Create()
		{
			return serializer ?? (serializer = new Serializer(activator, converters));
		}
	}
}
