using System.Threading.Tasks;
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
		
		public async Task To<TCommand>() 
			where TCommand : ICommand<TData>
		{
			var payload = new Payload { CommandType = typeof(TCommand), Data = data };

			var message = container.Resolve<IMessageComposer>().GetMessage(payload);
			
			var packets = await container.Resolve<IPacketsComposer>()
				.GetPackets(message)
				.ConfigureAwait(false);

			var transport = container.Resolve<ITransport>();
			
			foreach (var packet in packets)
			{
				await transport.WritePacket(packet).ConfigureAwait(false);
			}
		}
	}
}
