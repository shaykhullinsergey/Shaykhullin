using Shaykhullin.Activator.Core;

namespace Shaykhullin.Activator
{
	public class ActivatorConfig : IActivatorConfig
	{
		public IActivator Create()
		{
			return new Core.Activator();
		}
	}
}