using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Shaykhullin.ArrayPool
{
	internal class ArrayPool : IArrayPool
	{
		private bool disposed;
		private readonly ConcurrentDictionary<Type, Pool> pools = new ConcurrentDictionary<Type, Pool>();

		public TElement[] GetArray<TElement>(int length)
		{
			if (disposed)
			{
				throw new ObjectDisposedException(nameof(ArrayPool));
			}

			return GetPoolArray<TElement>(length, false);
		}
		
		public TElement[] GetArrayExact<TElement>(int length)
		{
			if (disposed)
			{
				throw new ObjectDisposedException(nameof(ArrayPool));
			}

			return GetPoolArray<TElement>(length, true);
		}

		private TElement[] GetPoolArray<TElement>(int length, bool exact)
		{
			if (disposed)
			{
				throw new ObjectDisposedException(nameof(ArrayPool));
			}
			
			var elementType = typeof(TElement);

			if (pools.TryGetValue(elementType, out var pool))
			{
				return GetLockedArray<TElement>(pool.Queues, length, exact);
			}

			var addPool = new Pool();
			addPool.Queues.Add(new Queue(length));
			pools[elementType] = addPool;
			return new TElement[length];
		}
		
		private static TElement[] GetLockedArray<TElement>(List<Queue> queues, int length, bool exact)
		{
			for (var i = 0; i < queues.Count; i++)
			{
				if (exact && queues[i].Length == length || !exact && queues[i].Length >= length)
				{
					var pool = queues[i];
					
					if (pool.Count > 0)
					{
						var array = pool.Dequeue();

						if (array != null)
						{
							return (TElement[])array;
						}
					}

					return new TElement[length];
				}
			}

			queues.Add(new Queue(length));
			return new TElement[length];
		}

		public void ReleaseArray<TElement>(TElement[] array)
		{
			if (disposed)
			{
				throw new ObjectDisposedException(nameof(ArrayPool));
			}
			
			var elementType = typeof(TElement);
			
			if (pools.TryGetValue(elementType, out var pool))
			{
				lock (pool.Lock)
				{
					for (var i = 0; i < pool.Queues.Count; i++)
					{
						if (pool.Queues[i].Length == array.Length)
						{
							pool.Queues[i].Enqueue(array);
							return;
						}
					}
					
					var queue = new Queue(array.Length);
					queue.Enqueue(array);
					pool.Queues.Add(queue);
				}
			}
		}

		public void Dispose()
		{
			if (!disposed)
			{
				disposed = true;

				pools.Clear();
			}
		}
	}
}