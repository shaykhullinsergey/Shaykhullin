using System;
using System.Reflection;

namespace Shaykhullin.DependencyInjection.Core
{
	internal class Dependency
	{
		public Type Register { get; }
		public Type Implemented { get; set; }
		public Func<IContainer, object> Factory { get; set; }
		
		public Type Lifecycle { get; set; }
		public ILifecycle Instance { get; set; }

		public Type For { get; set; }
		public Type[] Parameters { get; set; }

		public Dependency(Type register)
		{
			Register = register;
			Implemented = register;
			Lifecycle = typeof(Transient);
		}
	}
}