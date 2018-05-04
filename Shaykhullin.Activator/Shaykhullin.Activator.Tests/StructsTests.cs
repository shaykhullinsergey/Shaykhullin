using System;
using Xunit;

namespace Shaykhullin.Activator.Tests
{
	public class StructsTests
	{
		[Fact]
		public void ActivatorCreatesStructs()
		{
			var activator = new ActivatorConfig().Create();

			var int1 = activator.Create<int>();

			var int2 = activator.Create<int>();
			
			Assert.Equal(0, int1);
			Assert.Equal(0, int2);
		}
	}
}