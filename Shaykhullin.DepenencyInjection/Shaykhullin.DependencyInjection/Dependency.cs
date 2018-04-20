using System;

namespace Shaykhullin.DependencyInjection.Core
{
	internal class Dependency
	{
		public Type Registry { get; }
		public Type Implementation { get; set; }
		public Func<IContainer, object> Factory { get; set; }
		
		public Type Lifecycle { get; set; }
		public ILifecycle Instance { get; set; }

		public Type For { get; set; }
		public Type[] Parameters { get; set; }

		public Dependency(Type registry)
		{
			Registry = registry;
			Implementation = registry;
			Lifecycle = typeof(Transient);
		}
	}
}