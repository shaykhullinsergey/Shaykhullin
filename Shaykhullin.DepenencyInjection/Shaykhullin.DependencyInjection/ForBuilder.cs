using System;

namespace Shaykhullin.DependencyInjection.Core
{
	internal class ForBuilder : IForBuilder
	{
		private readonly Dependency dependency;

		public ForBuilder(Dependency dependency)
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