using System;
using System.Threading.Tasks;

namespace Shaykhullin.Network.Core
{
	public interface IHandlerDto
	{
		Type HandlerType { get; }
	}
}