using System;
using Xunit;

namespace Shaykhullin.DependencyInjection.Tests
{
	public class StructTests
	{
		[Fact]
		public void StructResolves()
		{
			var config = new ContainerConfig();

			config.Register<int>();

			using (var container = config.Create())
			{
				Assert.Equal(0, container.Resolve<int>());
			}
		}

		[Fact]
		public void ImplementedStructResolves()
		{
			var config = new ContainerConfig();

			config.Register<int>()
				.ImplementedBy(c => 12);

			using (var container = config.Create())
			{
				Assert.Equal(12, container.Resolve<int>());
			}
		}
		
		public class IntHolder
		{
			public IntHolder(int int32)
			{
				Int32 = int32;
			}
			
			public int Int32 { get; set; }
		}

		[Fact]
		public void IntHolderResolvesWithIntAsADependency()
		{
			var config = new ContainerConfig();

			config.Register<IntHolder>();

			config.Register<int>()
				.ImplementedBy(c => 12);

			using (var container = config.Create())
			{
				var result = container.Resolve<IntHolder>();
				
				Assert.Equal(12, result.Int32);
			}
		}

		[Fact]
		public void IntHolderResolvesWithForInt32()
		{
			var config = new ContainerConfig();

			config.Register<IntHolder>();

			config.Register<int>()
				.ImplementedBy(c => 12);
			
			config.Register<int>()
				.ImplementedBy(c => 13)
				.For<IntHolder>();

			using (var container = config.Create())
			{
				var result = container.Resolve<IntHolder>();

				Assert.Equal(13, result.Int32);
			}
		}

		[Fact]
		public void IntAsInterfaceResolvesCorrect()
		{
			var config = new ContainerConfig();

			config.Register<IComparable>()
				.ImplementedBy(c => 12);

			using (var container = config.Create())
			{
				Assert.Equal(12, container.Resolve<IComparable>());
			}
		}
	}
}