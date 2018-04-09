using System;
using System.Collections.Generic;

using Shaykhullin.Activator;
using Shaykhullin.Serializer.Core;

namespace Shaykhullin.Serializer
{
	internal class UseBuilder<TData> : IUseBuilder<TData>
	{
		private IActivator activator;
		private Dictionary<Type, IConverter> converters;

		public UseBuilder(IActivator activator, Dictionary<Type, IConverter> converters)
		{
			this.activator = activator;
			this.converters = converters;
		}

		public void Use<TConverter>() 
			where TConverter : IConverter<TData>
		{
			var type = typeof(TData);

			if(converters.ContainsKey(type))
			{
				converters[type] = activator.Create<TConverter>();
			}
			else
			{
				converters.Add(type, activator.Create<TConverter>());
			}
		}
	}
}
