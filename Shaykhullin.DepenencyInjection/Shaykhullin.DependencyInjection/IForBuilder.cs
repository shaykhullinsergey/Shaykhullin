using System;

namespace Shaykhullin.DependencyInjection.Core
{
	public struct ForBuilder
	{
		private readonly Dependency dependency;

		internal ForBuilder(Dependency dependency)
		{
			this.dependency = dependency;
		}

		public void For<TDependency>()
		{
			For(typeof(TDependency));
		}

		public void For(Type type)
		{
			dependency.ForDependency = type;
		}
	}
}