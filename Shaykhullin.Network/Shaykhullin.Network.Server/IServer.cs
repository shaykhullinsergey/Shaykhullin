using System;
using System.Threading.Tasks;

namespace Shaykhullin.Network.Core
{
	public interface IServer : IApplication, IDisposable
	{
		Task Run();
	}
}