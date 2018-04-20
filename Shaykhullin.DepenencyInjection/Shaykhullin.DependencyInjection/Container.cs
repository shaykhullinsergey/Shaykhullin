using System;
using Shaykhullin.Activator;

namespace Shaykhullin.DependencyInjection.Core
{
	internal class Container : IContainer
	{
		private bool disposed;
		private readonly IActivator activator;
		private readonly DependencyContainer dependencyContainer;

		public Container(IActivator activator, DependencyContainer dependencyContainer)
		{
			this.activator = activator;
			this.dependencyContainer = dependencyContainer;
		}

		public TResolve Resolve<TResolve>()
		{
			if (disposed)
			{
				throw new ObjectDisposedException(nameof(Container));
			}
			
			return (TResolve)Resolve(typeof(TResolve));
		}

		public object Resolve(Type type)
		{
			if (disposed)
			{
				throw new ObjectDisposedException(nameof(Container));
			}

			if (type == null)
			{
				throw new ArgumentNullException(nameof(type));
			}
			
			return ResolveRecursive(type, null);
		}

		private object ResolveRecursive(Type registry, Type @for)
		{
			if (registry == null)
			{
				throw new ArgumentNullException(nameof(registry));
			}
			
			var dependency = dependencyContainer.Get(registry, @for) 
				?? throw new InvalidOperationException($"{registry} not found");

			EnsureLifecycle(dependency);

			if (dependency.Factory != null)
			{
				return dependency.Instance.Resolve(() => dependency.Factory(this));
			}

			EnsureParameters(dependency);
			
			var arguments = new object[dependency.Parameters.Length];
			for (var i = 0; i < arguments.Length; i++)
			{
				arguments[i] = ResolveRecursive(dependency.Parameters[i], registry);
			}

			return dependency.Instance.Resolve(dependency.Implementation, arguments);
		}

		private void EnsureLifecycle(Dependency dependency)
		{
			if (dependency.Instance == null)
			{
				dependency.Instance = (ILifecycle)activator.Create(dependency.Lifecycle, activator);
			}
		}

		private static void EnsureParameters(Dependency dependency)
		{
			if (dependency.Parameters == null)
			{
				var constructors = dependency.Implementation.GetConstructors();

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
			if (!disposed)
			{
				disposed = true;
			}
		}
	}
}