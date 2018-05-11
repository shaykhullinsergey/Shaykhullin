using Shaykhullin.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Shaykhullin.Network.Core
{
	internal class HandlerCollection
	{
		private readonly Dictionary<Type, List<IHandlerDto>> handlers = new Dictionary<Type, List<IHandlerDto>>();
		private readonly Dictionary<Type, List<IHandlerDto>> asyncHandlers = new Dictionary<Type, List<IHandlerDto>>();
		private readonly IContainerConfig config;

		public HandlerCollection(IContainerConfig config)
		{
			this.config = config;
		}

		public void Add<TData, TCommand, THandler>()
			where TCommand : ICommand<TData>
			where THandler : IHandler<TData, TCommand>
		{
			var command = typeof(TCommand);
			var handler = typeof(THandler);
			
			if (handlers.TryGetValue(command, out var list))
			{
				list.Add(new HandlerDto(handler));
			}
			else
			{
				config.Register<THandler>();
				handlers.Add(command, new List<IHandlerDto> { new HandlerDto(handler) });
			}
		}

		public void AddAsync<TData, TCommand, THandler>()
			where TCommand : ICommand<TData>
			where THandler : IAsyncHandler<TData, TCommand>
		{
			var command = typeof(TCommand);
			var handler = typeof(THandler);
			
			if (asyncHandlers.TryGetValue(command, out var list))
			{
				list.Add(new AsyncHandlerDto(handler));
			}
			else
			{
				config.Register<THandler>();
				asyncHandlers.Add(command, new List<IHandlerDto> { new AsyncHandlerDto(handler) });
			}
		}

		public IList<IHandlerDto> GetAsyncHandlers(Type command)
		{
			if(!asyncHandlers.TryGetValue(command, out var list))
			{
				list = new List<IHandlerDto>();
				asyncHandlers.Add(command, list);
			}

			return list;
		}
		
		public IList<IHandlerDto> GetHandlers(Type command)
		{
			if(!handlers.TryGetValue(command, out var list))
			{
				list = new List<IHandlerDto>();
				handlers.Add(command, list);
			}

			return list;
		}
	}
}