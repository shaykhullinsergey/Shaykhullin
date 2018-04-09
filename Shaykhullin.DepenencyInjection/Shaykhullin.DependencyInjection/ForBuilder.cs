using System;

namespace Shaykhullin.DependencyInjection.Core
{
	internal class ForBuilder : IForBuilder
	{
		private readonly Dependency dto;

		public ForBuilder(Dependency dto)
		{
			this.dto = dto;
		}

		public void For<TDependency>()
		{
			For(typeof(TDependency));
		}

		public void For(Type dependency)
		{
			dto.For = dependency;
		}
	}
}