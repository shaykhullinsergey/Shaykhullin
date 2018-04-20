using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Shaykhullin.DependencyInjection;

namespace Shaykhullin.Network.Core
{
	internal class Connection : IConnection
	{
		private bool disposed;
		private readonly IContainer container;

		public Connection(IContainerConfig config)
		{
			config.Register<IPacketsComposer>()
				.ImplementedBy<PacketsComposer>()
				.As<Singleton>();

			config.Register<IMessageComposer>()
				.ImplementedBy<MessageComposer>()
				.As<Singleton>();

			config.Register<IEventRaiser>()
				.ImplementedBy<EventRaiser>()
				.As<Singleton>();

			config.Register<ICommunicator>()
				.ImplementedBy<Communicator>()
				.As<Singleton>();

			container = config.Create();

			Task.Run(async () => await ReceiveLoop());
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
			var messages = new Dictionary<int, IList<IPacket>>();
			var communicator = container.Resolve<ICommunicator>();
			var packetsComposer = container.Resolve<IPacketsComposer>();
			var messageComposer = container.Resolve<IMessageComposer>();
			var eventRaiser = container.Resolve<IEventRaiser>();

			while (true)
			{
				var packet = await communicator.Receive().ConfigureAwait(false);

				if (messages.TryGetValue(packet.Id, out var packets))
				{
					packets.Add(packet);
				}
				else
				{
					packets = new List<IPacket> { packet };
					messages.Add(packet.Id, packets);
				}
				messages.Remove(packet.Id);

				if (packet.End)
				{
					Task.Run(async () =>
					{
						var message = await packetsComposer.GetMessage(packets).ConfigureAwait(false);
						var payload = await messageComposer.GetPayload(message).ConfigureAwait(false);

						await eventRaiser.Raise(payload).ConfigureAwait(false);
					}).ConfigureAwait(false);
				}
			}
		}

		public void Dispose()
		{
			if (!disposed)
			{
				disposed = true;
				container.Resolve<ICommunicator>().Dispose();
			}
		}
	}
}
