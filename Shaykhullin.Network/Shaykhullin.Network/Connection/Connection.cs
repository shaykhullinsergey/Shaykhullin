using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Shaykhullin.ArrayPool;
using Shaykhullin.DependencyInjection;

namespace Shaykhullin.Network.Core
{
	internal class Connection : IConnection
	{
		private bool disposed;
		private readonly IContainer container;
		private readonly Task receiveLoopTask;

		public Connection(IContainerConfig config)
		{
			config.Register<IPacketsComposer>()
				.ImplementedBy<PacketsComposer>()
				.As<Singleton>();

			config.Register<IMessageComposer>()
				.ImplementedBy<MessageComposer>()
				.As<Singleton>();

			config.Register<ICommandRaiser>()
				.ImplementedBy<CommandRaiser>()
				.As<Singleton>();

			config.Register<ITransport>()
				.ImplementedBy<Transport>()
				.As<Singleton>();

			container = config.Create();

			receiveLoopTask = Task.Run(ReceiveLoop);
		}

		public ISendBuilder<TData> Send<TData>(TData data)
		{
			if (disposed)
			{
				throw new ObjectDisposedException(nameof(Connection));
			}

			return new SendBuilder<TData>(container, data);
		}

		private async Task ReceiveLoop()
		{
			var messages = new Dictionary<byte, IList<IPacket>>();
			var transport = container.Resolve<ITransport>();
			var packetsComposer = container.Resolve<IPacketsComposer>();
			var messageComposer = container.Resolve<IMessageComposer>();
			var commandRaiser = container.Resolve<ICommandRaiser>();
			
			var listQueue = new Queue<IList<IPacket>>();
			
			while (true)
			{
				var packet = await transport.ReadPacket().ConfigureAwait(false);

				if (messages.TryGetValue(packet.Id, out var packets))
				{
					packets.Add(packet);
				}
				else
				{
					packets = listQueue.Count > 0 
						? listQueue.Dequeue() 
						: new List<IPacket>();
					
					packets.Add(packet);

					messages.Add(packet.Id, packets);
				}

				if (packet.IsLast)
				{
					messages.Remove(packet.Id);
					var message = packetsComposer.GetMessage(packets);
					var payload = messageComposer.GetPayload(message);

					for (var i = 0; i < packets.Count; i++)
					{
						packetsComposer.ReleaseBuffer(packets[i].Buffer);
					}
					
					packets.Clear();
					listQueue.Enqueue(packets);
					
					packetsComposer.ReleaseBuffer(message.DataStreamBuffer);
					
					await commandRaiser.RaiseCommand(payload).ConfigureAwait(false);
				}
			}
		}

		public void Dispose()
		{
			if (!disposed)
			{
				disposed = true;
				container.Resolve<ITransport>().Dispose();
				receiveLoopTask.Dispose();
			}
		}
	}
}