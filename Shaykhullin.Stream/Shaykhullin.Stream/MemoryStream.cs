using System;
using System.IO;
using Shaykhullin.ArrayPool;

namespace Shaykhullin.Stream
{
	public class MemoryStream : System.IO.Stream
	{
		public byte[] Buffer { get; private set; }
		private IArrayPool arrayPool;
		
		public MemoryStream()
		{
			Buffer = Array.Empty<byte>();
			arrayPool = new ArrayPoolConfig().Create();
		}

		public MemoryStream(IArrayPool arrayPool)
		{
			Buffer = Array.Empty<byte>();
			this.arrayPool = arrayPool;
		}

		public MemoryStream(byte[] buffer)
		{
			Buffer = buffer;
		}

		private long pos = 0;

		public override long Position
		{
			get => pos;
			set => pos = value;
		}

		public override int Read(byte[] destination, int offset, int count)
		{
			CopyTo(Buffer, Position, destination, offset, count);
			return count;
		}

		public override void Write(byte[] source, int offset, int count)
		{
			EnsureCapacity(count);
			CopyTo(source, offset, Buffer, Position, count);
		}

		public override void WriteByte(byte value)
		{
			EnsureCapacity(1);
			Buffer[Position++] = value;
		}

		public override int ReadByte()
		{
			return Buffer[Position++];
		}

		private void EnsureCapacity(int count)
		{
			if (Buffer.Length < Position + count)
			{
				var length = CalculateLength(Buffer.Length, (int)Position + count);
				
				if (Buffer.Length != 0)
				{
					var poolBuffer = arrayPool.GetArrayExact<byte>(length);
					Array.Copy(Buffer, poolBuffer, Buffer.Length);
					
					arrayPool.ReleaseArray(Buffer);
					Buffer = poolBuffer;
				}
				else
				{
					Buffer = arrayPool.GetArrayExact<byte>(length);
				}
			}
		}

		private static int CalculateLength(int current, int count)
		{
			var length = current == 0 ? 1 : current;

			while (length < count)
			{
				length *= 2;
			}

			return length;
		}

		private void CopyTo(byte[] source, long from, byte[] destination, long to, int count)
		{
			for (var index = 0; index < count; index++)
			{
				destination[to + index] = source[from + index];
			}
			
			Position += count;
		}

		protected override void Dispose(bool disposing)
		{
			arrayPool?.ReleaseArray(Buffer);
			Buffer = null;
			arrayPool = null;
			
			base.Dispose(disposing);
		}

		public override bool CanRead { get; } = true;
		public override bool CanSeek { get; } = false;
		public override bool CanWrite { get; } = true;
		public override long Length => Buffer.Length;
		public override void Flush() => throw new NotImplementedException();
		public override void SetLength(long value) => throw new NotImplementedException();
		public override long Seek(long offset, SeekOrigin origin) => throw new NotImplementedException();
	}
}