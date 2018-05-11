using System;
using Shaykhullin.ArrayPool;

namespace Shaykhullin.Stream
{
	public struct ValueStream : IDisposable
	{
		private IArrayPool arrayPool;

		public byte[] Buffer { get; private set; }
		
		public ValueStream(IArrayPool arrayPool)
		{
			Buffer = Array.Empty<byte>();
			this.arrayPool = arrayPool;
			Position = 0;
		}

		public ValueStream(byte[] buffer)
		{
			Buffer = buffer;
			arrayPool = null;
			Position = 0;
		}

		public long Position { get; set; }

		public void Seek(long position)
		{
			Position = position;
		}

		public int Read(byte[] destination, int offset, int count)
		{
			CopyTo(Buffer, Position, destination, offset, count);
			return count;
		}
		
		public int ReadByte()
		{
			return Buffer[Position++];
		}

		public int ReadInt32()
		{
			return new UnifiedUnion(
				Buffer[Position++],
				Buffer[Position++],
				Buffer[Position++],
				Buffer[Position++]).Int32;
		}
		
		public void Write(byte[] source, int offset, int count)
		{
			EnsureCapacity(count);
			CopyTo(source, offset, Buffer, Position, count);
		}
		
		public void WriteByte(byte value)
		{
			EnsureCapacity(1);
			Buffer[Position++] = value;
		}

		public void WriteInt32(int value)
		{
			EnsureCapacity(4);
			
			var union = new UnifiedUnion(value);
			Buffer[Position++] = union.Byte1;
			Buffer[Position++] = union.Byte2;
			Buffer[Position++] = union.Byte3;
			Buffer[Position++] = union.Byte4;
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
			var length = current == 0 ? 8 : current;

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
		
		public void Dispose()
		{
			arrayPool?.ReleaseArray(Buffer);
			Buffer = null;
			arrayPool = null;
		}

		public bool CanRead => true;
		public bool CanSeek => false;
		public bool CanWrite => true;
		public long Length => Buffer.Length;
	}
}