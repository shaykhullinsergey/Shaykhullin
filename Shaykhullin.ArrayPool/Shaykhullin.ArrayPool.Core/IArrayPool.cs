using System;

namespace Shaykhullin.ArrayPool
{
	public interface IArrayPool : IDisposable
	{
		TElement[] GetArray<TElement>(int length);
		TElement[] GetArrayExact<TElement>(int length);
		void ReleaseArray<TElement>(TElement[] array);
	}
}