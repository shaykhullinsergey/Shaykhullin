using System;
using System.Collections.Generic;
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

		public class Test
		{
			public int Id { get; set; }
		}
		
		public class NextTest : Test
		{
			public int NextId { get; set; }
		}
		
		[Fact]
		public void Performance()
		{
			var config = new ContainerConfig();

			config.Register<Test>()
				.ImplementedBy<NextTest>()
				.As<Singleton>();

			using (var container = config.Create())
			{
				var list = new List<Test>();
				
				for (var i = 0; i < 10_000; i++)
				{
					list.Add(container.Resolve<Test>());
				}
			}
		}
	}
}