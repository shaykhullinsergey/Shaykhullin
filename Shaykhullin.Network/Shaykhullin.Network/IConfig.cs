using System;
using Shaykhullin.DependencyInjection.Core;

namespace Shaykhullin.Network.Core
{
	public interface IConfig<out TNode> : IConfigurationBuilder
		where TNode : IApplication
	{
		CommandBuilder<TData> On<TData>();
		
		ImplementedByBuilder<TRegistry> Register<TRegistry>();
		ImplementedByBuilder<TRegistry> Register<TRegistry>(Type registry);
		ImplementedByBuilder<object> Register(Type registry);
		
		TNode Create(string host, int port);
	}
}