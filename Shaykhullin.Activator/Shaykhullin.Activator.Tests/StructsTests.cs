using System;
using Xunit;

namespace Shaykhullin.Activator.Tests
{
	public class StructsTests
	{
		class A
		{}
		
		
		[Fact]
		public void ActivatorCreatesStructs()
		{
			var activator = new ActivatorConfig().Create();

			var int1 = activator.Create<A>();

			var int2 = activator.Create<A>();
		}

		class B
		{
			public A A { get; set; }

			public B(A a)
			{
				A = a;
			}
		}
		
		[Fact]
		public void ActivatorWithParameters()
		{
			var activator = new ActivatorConfig().Create();

			var a = new object[]{new A()};
			
			
			var b = activator.Create<B>(a);
			
			Assert.Same(a[0], b.A);
		}
	}
}