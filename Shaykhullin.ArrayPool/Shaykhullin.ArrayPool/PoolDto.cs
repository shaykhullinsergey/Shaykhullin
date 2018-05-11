using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Shaykhullin.ArrayPool
{
	internal class Pool
	{
		public object Lock { get; }
		public List<Queue> Queues { get; }

		public Pool()
		{
			Lock = new object();
			Queues = new List<Queue>();
		}
	}

	internal class Queue
	{
		private readonly Queue<Array> queue = new Queue<Array>();
		
		public Queue(int length)
		{
			Length = length;
		}
		
		public int Length { get; }
		public int Count => queue.Count;
		
		public void Enqueue(Array array)
		{
			queue.Enqueue(array);
		}

		public Array Dequeue()
		{
			return queue.Count > 0 ? queue.Dequeue() : null;
		}
	}
}