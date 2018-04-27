using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
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
			var communicator = container.Resolve<ITransport>();
			var packetsComposer = container.Resolve<IPacketsComposer>();
			var messageComposer = container.Resolve<IMessageComposer>();
			var commandRaiser = container.Resolve<ICommandRaiser>();

			while (true)
			{
				var packet = await communicator.ReadPacket().ConfigureAwait(false);

				if (messages.TryGetValue(packet.Id, out var packets))
				{
					packets.Add(packet);
				}
				else
				{
					packets = new List<IPacket>
					{
						packet
					};
					messages.Add(packet.Id, packets);
				}


				if (packet.IsLast)
				{
					messages.Remove(packet.Id);
					var message = packetsComposer.GetMessage(packets);
					var payload = messageComposer.GetPayload(message);

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