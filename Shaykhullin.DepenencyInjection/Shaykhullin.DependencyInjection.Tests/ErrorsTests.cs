using System;
using Xunit;

namespace Shaykhullin.DependencyInjection.Tests
{
	public class ErrorsTests
	{
		class Entity
		{
		}

		[Fact]
		public void IfNotRegisteredWorks()
		{
			using (var container = new ContainerConfig().Create())
			{
				Assert.Throws<InvalidOperationException>(() => container.Resolve<Entity>());
			}
		}
	}
}