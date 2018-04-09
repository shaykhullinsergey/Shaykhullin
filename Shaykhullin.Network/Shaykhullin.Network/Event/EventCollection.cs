using Shaykhullin.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Shaykhullin.Network.Core
{
	public class EventCollection
	{
		private readonly Dictionary<int, Type> events = new Dictionary<int, Type>();
		private readonly IContainerConfig config;

		public EventCollection(IContainerConfig config)
		{
			this.config = config;
		}

		public void Add<TData, TEvent>()
			where TEvent : IEvent<TData>
		{
			var @event = typeof(TEvent);
			
			if (!events.ContainsValue(@event))
			{
				config.Register<TEvent>();
				events.Add(GetHash(@event.Name), @event);
			}
		}
		
		public Type GetEvent(int id)
		{
			return events[id];
		}

		public int GetEvent(Type @event)
		{
			foreach (var item in events)
			{
				if (item.Value == @event)
				{
					return item.Key;
				}
			}

			var hash = GetHash(@event.Name);
			events.Add(hash, @event);
			config.Register<object>(@event);
			return hash;
		}
		
		private static unsafe int GetHash(string name)
		{
			fixed (char* str = name)
			{
				var chPtr = str;
				var num = 352654597;
				var num2 = num;
				var numPtr = (int*)chPtr;
				for (var i = name.Length; i > 0; i -= 4)
				{
					num = (num << 5) + num + (num >> 27) ^ numPtr[0];
					if (i <= 2)
					{
						break;
					}
					num2 = (num2 << 5) + num2 + (num2 >> 27) ^ numPtr[1];
					numPtr += 2;
				}
				return num + num2 * 1566083941;
			}
		}
	}
}