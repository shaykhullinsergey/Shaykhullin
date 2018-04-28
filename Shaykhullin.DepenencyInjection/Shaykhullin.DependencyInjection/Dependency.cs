using System;
using System.Collections.Generic;
using System.Reflection;
using Shaykhullin.Activator;

namespace Shaykhullin.DependencyInjection.Core
{
	internal class Dependency
	{
		private static readonly Type TransientLifecycleType = typeof(Transient);
		private static readonly Dictionary<Type, Type[]> ParameterCache = new Dictionary<Type, Type[]>();
		
		public Type Registry { get; }
		public Type Implementation { get; set; }
		public Type ForDependency { get; set; }
		
		public Func<IContainer, object> Factory { get; set; }
		
		public Type LifecycleType { get; set; }
		public ILifecycle Lifecycle { get; set; }

		public Type[] ConstructorParameters { get; set; }

		public Dependency(Type registry)
		{
			Registry = registry;
			Implementation = registry;
			LifecycleType = TransientLifecycleType;
		}
		
		public void EnsureLifecycle(IActivator activator)
		{
			if (Lifecycle == null)
			{
				Lifecycle = (ILifecycle)activator.Create(LifecycleType, activator);
			}
		}
		
		public void EnsureParameters()
		{
			if (ConstructorParameters != null)
			{
				return;
			}

			if (ParameterCache.TryGetValue(Implementation, out var parameters))
			{
				ConstructorParameters = parameters;
				return;
			}

			var constructors = Implementation.GetConstructors();

			if (constructors.Length == 0)
			{
				ConstructorParameters = Array.Empty<Type>();
				ParameterCache.Add(Implementation, ConstructorParameters);
			}
			else
			{
				var parametersInfo = constructors[0].GetParameters();

				parameters = parametersInfo.Length == 0
					? Array.Empty<Type>()
					: new Type[parametersInfo.Length];

				for (var i = 0; i < parametersInfo.Length; i++)
				{
					parameters[i] = parametersInfo[i].ParameterType;
				}

				ConstructorParameters = parameters;
				ParameterCache.Add(Implementation, ConstructorParameters);
			}
		}
	}
}