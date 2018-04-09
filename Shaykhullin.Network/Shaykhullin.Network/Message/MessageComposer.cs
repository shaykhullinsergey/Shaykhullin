using System.IO;
using System.Threading.Tasks;
using Shaykhullin.DependencyInjection;
using Shaykhullin.Serializer;

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
			byte[] data;
			using (var stream = new MemoryStream())
			{
				serializer.Serialize(stream, payload.Data);
				data = stream.ToArray();
			}

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

			object @object;

			using (var stream = new MemoryStream(data))
			{
				@object = serializer.Deserialize(stream, dataType);
			}

			return await Task.FromResult((IPayload)new Payload { Event = @event, Data = @object }).ConfigureAwait(false);
		}
	}
}