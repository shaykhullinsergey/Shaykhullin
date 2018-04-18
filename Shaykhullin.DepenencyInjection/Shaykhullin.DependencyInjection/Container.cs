using System;
using Shaykhullin.Activator;

namespace Shaykhullin.DependencyInjection.Core
{
	internal class Container : IContainer
	{
		private readonly IActivator activator;
		private readonly DependencyContainer dependencyContainer;

		public Container(IActivator activator, DependencyContainer dependencyContainer)
		{
			this.activator = activator;
			this.dependencyContainer = dependencyContainer;
		}

		public TResolve Resolve<TResolve>()
			where TResolve : class
		{
			return (TResolve)Resolve(typeof(TResolve));
		}

		public object Resolve(Type type)
		{
			return ResolveRecursive(type, null);
		}

		private object ResolveRecursive(Type register, Type @for)
		{
			var dependency = dependencyContainer.Get(register, @for) 
				?? throw new InvalidOperationException($"{register} not found");

			EnsureLifecycle(dependency);

			if (dependency.Factory != null)
			{
				return dependency.Instance.Resolve(() => dependency.Factory(this));
			}

			EnsureParameters(dependency);
			
			var arguments = new object[dependency.Parameters.Length];
			for (var i = 0; i < arguments.Length; i++)
			{
				arguments[i] = ResolveRecursive(dependency.Parameters[i], register);
			}

			return dependency.Instance.Resolve(dependency.Implemented, arguments);
		}

		private void EnsureLifecycle(Dependency dependency)
		{
			if (dependency.Instance == null)
			{
				dependency.Instance = (ILifecycle)activator.Create(dependency.Lifecycle, activator);
			}
		}

		private void EnsureParameters(Dependency dependency)
		{
			if (dependency.Parameters == null)
			{
				var constructors = dependency.Implemented.GetConstructors();

				if (constructors.Length == 0)
				{
					dependency.Parameters = Array.Empty<Type>();
				}
				else
				{
					var parameters = constructors[0].GetParameters();

					dependency.Parameters = new Type[parameters.Length];
					for (var i = 0; i < parameters.Length; i++)
					{
						dependency.Parameters[i] = parameters[i].ParameterType;
					}
				}
			}
		}

		public void Dispose()
		{
		}
	}
}