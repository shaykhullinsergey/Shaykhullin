using Shaykhullin.DependencyInjection.Core;

namespace Network.Core
{
	public interface IConfig<TNode> : IConfigurationBuilder, IRegisterBuilder, IEventBuilder
		where TNode : INode
	{
		TNode Create(string host, int port);
	}
}