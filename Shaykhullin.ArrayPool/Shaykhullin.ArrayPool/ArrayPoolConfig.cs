﻿using Shaykhullin.ArrayPool.Core;

namespace Shaykhullin.ArrayPool
{
	public class ArrayPoolConfig : IArrayPoolConfig
	{
		public IArrayPool Create()
		{
			return new ArrayPool();
		}
	}
}