using System.Threading.Tasks;
using Shaykhullin.ArrayPool;
using Shaykhullin.DependencyInjection;

namespace Shaykhullin.Network.Core
{
	internal class SendBuilder<TData> : ISendBuilder<TData>
	{
		private readonly IContainer container;
		private readonly TData data;

		public SendBuilder(IContainer container, TData data)
		{
			this.container = container;
			this.data = data;
		}

		public void To<TCommand>() where TCommand : ICommand<TData>
		{
			var payload = new Payload<TData> { CommandType = typeof(TCommand), Data = data };

			var message = container.Resolve<IMessageComposer>().GetMessage(payload);

			var packetsComposer = container.Resolve<IPacketsComposer>();

			var packets = packetsComposer.GetPackets(message);

			var transport = container.Resolve<ITransport>();

			for (var i = 0; i < packets.Length; i++)
			{
				transport.WritePacket(packets[i]);
				packetsComposer.ReleaseBuffer(packets[i].Buffer);
			}
			
			container.Resolve<IArrayPool>().ReleaseArray(packets);
		}

		public async Task ToAsync<TCommand>() 
			where TCommand : ICommand<TData>
		{
			var payload = new Payload<TData> { CommandType = typeof(TCommand), Data = data };

			var message = container.Resolve<IMessageComposer>().GetMessage(payload);

			var packetsComposer = container.Resolve<IPacketsComposer>();

			var packets = packetsComposer.GetPackets(message);

			var transport = container.Resolve<ITransport>();

			for (var i = 0; i < packets.Length; i++)
			{
				await transport.WritePacketAsync(packets[i]).ConfigureAwait(false);
				packetsComposer.ReleaseBuffer(packets[i].Buffer);
			}
			
			container.Resolve<IArrayPool>().ReleaseArray(packets);
		}
	}
}
