using System;
using System.Threading.Tasks;

namespace Shaykhullin.Network.Core
{
	public interface IHandlerDto
	{
		Type HandlerType { get; }
		Func<object, object, Task> ExecuteMethod { get; }
	}
}