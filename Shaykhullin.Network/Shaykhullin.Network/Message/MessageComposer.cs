using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Shaykhullin.DependencyInjection;

namespace Network.Core
{
	internal class MessageComposer : IMessageComposer
	{
		private readonly ISerializer serializer;
		private readonly ICompression compression;
		private readonly IEncryption encryption;
		private readonly IEventHolder eventHolder;

		public MessageComposer(IContainer container)
		{
			serializer = container.Resolve<ISerializer>();
			compression = container.Resolve<ICompression>();
			encryption = container.Resolve<IEncryption>();
			eventHolder = container.Resolve<IEventHolder>();
		}

		public async Task<IMessage> GetMessage(IPayload payload)
		{
			var data = serializer.Serialize(payload.Data);
			data = compression.Compress(data);
			data = encryption.Encrypt(data);

			var eventId = eventHolder.GetEvent(payload.Event);

			return await Task.FromResult((IMessage)new Message { EventId = eventId, Data = data }).ConfigureAwait(false);
		}

		public async Task<IPayload> GetPayload(IMessage message)
		{
			var data = encryption.Decrypt(message.Data);
			data = compression.Decompress(data);

			var @event = eventHolder.GetEvent(message.EventId);
			var dataType = @event.GetInterfaces()[0].GetGenericArguments()[0];

			var @object = serializer.Deserialize(data, dataType);

			return await Task.FromResult((IPayload)new Payload { Event = @event, Data = @object }).ConfigureAwait(false);
		}
	}
}