using System;
using System.Reflection;
using Shaykhullin.Activator;
using Shaykhullin.ArrayPool;

namespace Shaykhullin.DependencyInjection.Core
{
	internal class Container : IContainer
	{
		private bool disposed;
		private readonly IActivator activator;
		private readonly DependencyContainer dependencyContainer;
		private readonly IArrayPool arrayPool;

		public Container(IActivator activator, DependencyContainer dependencyContainer, IArrayPool arrayPool)
		{
			this.activator = activator ?? throw new ArgumentNullException(nameof(activator));
			this.dependencyContainer = dependencyContainer ?? throw new ArgumentNullException(nameof(dependencyContainer));
			this.arrayPool = arrayPool;
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
			if (type == null)
			{
				throw new ArgumentNullException(nameof(type));
			}

			if (disposed)
			{
				throw new ObjectDisposedException(nameof(Container));
			}

			return ResolveRecursive(type, null);
		}

		private object ResolveRecursive(Type registry, Type @for)
		{
			if (registry == null)
			{
				throw new ArgumentNullException(nameof(registry));
			}

			var dependency = dependencyContainer.TryGetDependency(registry, @for)
				?? throw new InvalidOperationException($"{registry} not found");

			if (dependency.Instance != null)
			{
				return dependency.Instance;
			}
			
			dependency.EnsureLifecycle(activator);
			
			if (dependency.Factory != null)
			{
				var factory = dependency.Factory;
				return dependency.Lifecycle.Resolve(factory, this);
			}

			dependency.EnsureParameters();

			var arguments = CreateArguments(registry, dependency.ConstructorParameters);

			var instance = dependency.Lifecycle.Resolve(dependency.Implementation, arguments); 
			
			arrayPool.ReleaseArray(arguments);

			return instance;
		}

		private object[] CreateArguments(Type registry, Type[] parameters)
		{
			var arguments = parameters.Length == 0
				? Array.Empty<object>()
				: arrayPool.GetArrayExact<object>(parameters.Length);

			for (var i = 0; i < arguments.Length; i++)
			{
				arguments[i] = ResolveRecursive(parameters[i], registry);
			}

			return arguments;
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