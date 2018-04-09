using System.Threading.Tasks;
using Shaykhullin.DependencyInjection;

namespace Shaykhullin.Network.Core
{
	internal class SendBuilder<TData> : ISendBuilder<TData>
	{
		private readonly IContainer container;
		private readonly object data;

		public SendBuilder(IContainer container, object data)
		{
			this.container = container;
			this.data = data;
		}
		
		public async Task To<TEvent>() 
			where TEvent : IEvent<TData>
		{
			var payload = new Payload { Event = typeof(TEvent), Data = data };
			
			var message = await container.Resolve<IMessageComposer>()
				.GetMessage(payload).ConfigureAwait(false);
			
			var packets = await container.Resolve<IPacketsComposer>()
				.GetPackets(message).ConfigureAwait(false);

			var communicator = container.Resolve<ICommunicator>();
			
			foreach (var packet in packets)
			{
				await communicator.Send(packet).ConfigureAwait(false);
			}
		}
	}
}
