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
			dependencies = new List<Dependency>(1);
		}

		public Dependency Register(Type register)
		{
			var dto = new Dependency(register);
			dependencies.Add(dto);
			return dto;
		}

		public Dependency TryGetDependency(Type register, Type forDependency = null)
		{
			if (forDependency == null)
			{
				for (var i = 0; i < dependencies.Count; i++)
				{
					if (dependencies[i].Registry == register && dependencies[i].ForDependency == null)
					{
						return dependencies[i];
					}
				}
			}
			else
			{
				Dependency nullDependency = null;

				for (var i = 0; i < dependencies.Count; i++)
				{
					if (dependencies[i].Registry == register && dependencies[i].ForDependency == forDependency)
					{
						return dependencies[i];
					}
					
					if (dependencies[i].Registry == register && dependencies[i].ForDependency == null)
					{
						nullDependency = dependencies[i];
					}
				}

				if (nullDependency != null)
				{
					return nullDependency;
				}
			}

			return parent?.TryGetDependency(register, forDependency);
		}
	}
}