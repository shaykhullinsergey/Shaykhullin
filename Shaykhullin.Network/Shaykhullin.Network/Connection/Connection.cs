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

		private readonly List<MessageDto> messages = new List<MessageDto>();
		private readonly Queue<IList<Packet>> queue = new Queue<IList<Packet>>();
		
		private Task ReceiveLoop()
		{
			var transport = container.Resolve<ITransport>();
			var packetsComposer = container.Resolve<IPacketsComposer>();
			var messageComposer = container.Resolve<IMessageComposer>();
			var commandRaiser = container.Resolve<ICommandRaiser>();

			while (true)
			{
				var packet = transport.ReadPacket();

				var messageDto = EnsurePacket(packet);
				
				if (packet.IsLast)
				{
					var packets = messageDto.Packets;
					messages.Remove(messageDto);
					var message = packetsComposer.GetMessage(packets);
					var payload = messageComposer.GetPayload<object>(message);

					for (var i = 0; i < packets.Count; i++)
					{
						packetsComposer.ReleaseBuffer(packets[i].Buffer);
					}

					packets.Clear();
					queue.Enqueue(packets);

					packetsComposer.ReleaseBuffer(message.Data);

					commandRaiser.RaiseCommand(payload).GetAwaiter().GetResult();
				}
			}
		}

		private MessageDto EnsurePacket(Packet packet)
		{
			for (var i = 0; i < messages.Count; i++)
			{
				if (messages[i].MessageId == packet.MessageId)
				{
					messages[i].Packets.Add(packet);
					return messages[i];
				}
			}

			var packets = queue.Count > 0
				? queue.Dequeue()
				: new List<Packet>();
			
			packets.Add(packet);
			var messageDto = new MessageDto(packet.MessageId, packets);
			messages.Add(messageDto);
			return messageDto;
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