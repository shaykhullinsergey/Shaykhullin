using Shaykhullin.DependencyInjection.Core;

namespace Shaykhullin.Network.Core
{
	public interface IConfig<TNode> : IConfigurationBuilder, IRegisterBuilder, IDataBuilder
		where TNode : INode
	{
		TNode Create(string host, int port);
	}
}