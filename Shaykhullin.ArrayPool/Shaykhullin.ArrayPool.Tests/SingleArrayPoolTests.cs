using Xunit;
using System.Linq;

namespace Shaykhullin.ArrayPool.Tests
{
	public class UnitTest1
	{
		[Fact]
		public void ArrayPoolUsesSameArray()
		{
			var config = new ArrayPoolConfig();
			var pool = config.Create();

			var array1 = pool.GetArrayExact<object>(1);
			pool.ReleaseArray(array1);

			var array2 = pool.GetArrayExact<object>(1);
			
			Assert.Equal(array1, array2);
		}

		[Fact]
		public void ArrayPoolCuncurrentPerformance()
		{
			var config = new ArrayPoolConfig();
			var pool = config.Create();
			
			Enumerable.Range(0, 100)
				.AsParallel()
				.WithDegreeOfParallelism(20)
				.ForAll(x =>
				{
					var array = pool.GetArrayExact<object>(1);
					array[0] = x;
					pool.ReleaseArray(array);
				});
		}
	}
}