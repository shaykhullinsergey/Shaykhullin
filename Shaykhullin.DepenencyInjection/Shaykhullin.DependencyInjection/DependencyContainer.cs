using System;
using System.Collections.Generic;
using System.Linq;
using Shaykhullin.DependencyInjection.Core;

namespace Shaykhullin.DependencyInjection
{
	internal class DependencyContainer
	{
		private readonly DependencyContainer parent;
		private readonly IList<Dependency> dependencies;

		public DependencyContainer(DependencyContainer parent = null)
		{
			this.parent = parent;
			dependencies = new List<Dependency>();
		}

		public Dependency Register(Type register)
		{
			var dto = new Dependency(register);
			dependencies.Add(dto);
			return dto;
		}

		public Dependency TryGetDependency(Type register, Type @for = null)
		{
			if (@for == null)
			{
				foreach (var dependency in dependencies.Where(dependency => dependency.Registry == register && dependency.For == null))
				{
					return dependency;
				}
			}
			else
			{
				Dependency forNullFor = null;

				foreach (var dependency in dependencies)
				{
					if (dependency.Registry == register && dependency.For == @for)
					{
						return dependency;
					}

					if (dependency.Registry == register && dependency.For == null)
					{
						forNullFor = dependency;
					}
				}

				if (forNullFor != null)
				{
					return forNullFor;
				}
			}

			return parent?.TryGetDependency(register, @for);
		}
	}
}