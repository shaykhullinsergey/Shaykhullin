using System.Threading.Tasks;

namespace Shaykhullin.Network.Core
{
	public interface ISendBuilder<TData>
	{
		void To<TCommand>()
			where TCommand : ICommand<TData>;
		
		Task ToAsync<TCommand>()
			where TCommand : ICommand<TData>;
	}
}
