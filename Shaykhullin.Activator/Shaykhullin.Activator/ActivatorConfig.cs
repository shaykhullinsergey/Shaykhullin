using Shaykhullin.ArrayPool;
using Shaykhullin.Activator.Core;

namespace Shaykhullin.Activator
{
	public class ActivatorConfig : IActivatorConfig
	{
		private readonly IArrayPool arrayPool;

		public ActivatorConfig()
		{
			arrayPool = new ArrayPoolConfig().Create();
		}
		
		public IActivator Create()
		{
			return new Core.Activator(arrayPool);
		}
	}
}