using Shaykhullin.Serializer.Core;
using Shaykhullin.DependencyInjection;

namespace Shaykhullin.Serializer
{
	internal class UseBuilder<TData> : IUseBuilder<TData>
	{
		private readonly IContainerConfig scope;
		private readonly Configuration converters;

		public UseBuilder(IContainerConfig scope, Configuration converters)
		{
			this.scope = scope;
			this.converters = converters;
		}

		public void With<TConverter>() 
			where TConverter : IConverter<TData>
		{
			converters.RegisterConverterTypeFor(typeof(TData), typeof(TConverter));

			scope.Register<TConverter>()
				.As<Singleton>();
		}
	}
}
