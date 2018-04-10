using Shaykhullin.Serializer.Core;
using Shaykhullin.DependencyInjection;

namespace Shaykhullin.Serializer
{
	internal class UseBuilder<TData> : IUseBuilder<TData>
	{
		private readonly IContainerConfig scope;
		private ConverterCollection converters;

		public UseBuilder(IContainerConfig scope, ConverterCollection converters)
		{
			this.scope = scope;
			this.converters = converters;
		}

		public void Use<TConverter>() 
			where TConverter : IConverter<TData>
		{
			converters.Add(typeof(TData), typeof(TConverter), scope);
		}
	}
}
