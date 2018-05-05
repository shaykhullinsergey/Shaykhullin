using System;
using Shaykhullin.ArrayPool;
using Xunit;

namespace Shaykhullin.Stream.Tests
{
	public class UnitTest1
	{
		[Fact]
		public void StreamReadsAndWrites()
		{
			var input = new byte[] { 1, 2, 3, 4, 5 };
			
			using (var stream = new MemoryStream())
			{
				stream.Write(input, 0, input.Length);

				stream.Position = 0;
				
				var result = new byte[5];
				var read = stream.Read(result, 0, result.Length);
				
				Assert.Equal(result.Length, read);
				Assert.Equal(input, result);
			}
		}

		[Fact]
		public void ReadByteWriteByteWorks()
		{
			using (var stream = new MemoryStream())
			{
				stream.WriteByte(1);
				stream.WriteByte(2);
				stream.WriteByte(3);
				stream.WriteByte(4);
				stream.WriteByte(5);

				stream.Position = 0;
				
				Assert.Equal(1, stream.ReadByte());
				Assert.Equal(2, stream.ReadByte());
				Assert.Equal(3, stream.ReadByte());
				Assert.Equal(4, stream.ReadByte());
				Assert.Equal(5, stream.ReadByte());
			}
		}

		[Fact]
		public void DoubleResizing()
		{
			var input = new byte[] { 1, 2, 3, 4, 5 };
			using (var stream = new MemoryStream())
			{
				stream.Write(input, 0, input.Length);
				stream.Write(input, 0, input.Length);
				
				Assert.Equal(10, stream.Position);

				stream.Position = 0;

				var result = new byte[5];
				stream.Read(result, 0, result.Length);
				Assert.Equal(input, result);

				stream.Read(result, 0, result.Length);
				Assert.Equal(input, result);
			}
		}

		[Fact]
		public void DoubleResizingOnDifferentSize()
		{
			using (var stream = new MemoryStream())
			{
				stream.Write(new byte[10], 0, 10);
				stream.Write(new byte[20], 0, 20);
				stream.Write(new byte[50], 0, 50);
				
				Assert.Equal(80, stream.Position);
				Assert.Equal(128, stream.Length);
			}
		}

		[Fact]
		public void AllocationTests()
		{
			var input1 = new byte[5];
			var input2 = new byte[60];
			var input3 = new byte[100];
			var input4 = new byte[130];
			
			// 42 2 1
			var gg1 = GC.CollectionCount(0);
			var gg2 = GC.CollectionCount(1);
			var gg3 = GC.CollectionCount(2);
			
			using (var pool = new ArrayPoolConfig().Create())
			{
				for (var i = 0; i < 100_000_0; i++)
				{
					using (var stream = new MemoryStream(pool))
					{
						stream.Write(input1, 0, input1.Length);
						stream.Write(input2, 0, input2.Length);
						stream.Write(input3, 0, input3.Length);
						stream.Write(input4, 0, input4.Length);
					}
				}
			}

			var g1 = GC.CollectionCount(0);
			var g2 = GC.CollectionCount(1);
			var g3 = GC.CollectionCount(2);
			
			for (var i = 0; i < 100_000_0; i++)
			{
				using (var stream = new System.IO.MemoryStream())
				{
					stream.Write(input1, 0, input1.Length);
					stream.Write(input2, 0, input2.Length);
					stream.Write(input3, 0, input3.Length);
					stream.Write(input4, 0, input4.Length);
				}
			}
			
			// 565 2 1
			var g11 = GC.CollectionCount(0);
			var g22 = GC.CollectionCount(1);
			var g33 = GC.CollectionCount(2);
		}
	}
}