using System.IO;
using Shaykhullin.Serializer;
using Shaykhullin.DependencyInjection;

namespace Shaykhullin.Network.Core
{
	internal class MessageComposer : IMessageComposer
	{
		private readonly ISerializer serializer;
		private readonly ICompression compression;
		private readonly IEncryption encryption;
		private readonly ICommandHolder commandHolder;

		public MessageComposer(IContainer container)
		{
			serializer = container.Resolve<ISerializer>();
			compression = container.Resolve<ICompression>();
			encryption = container.Resolve<IEncryption>();
			commandHolder = container.Resolve<ICommandHolder>();
		}

		public IMessage GetMessage(IPayload payload)
		{
			byte[] data;

			using (var stream = new MemoryStream())
			{
				serializer.Serialize(stream, payload.Data);
				data = stream.ToArray();
			}

			data = compression.Compress(data);
			data = encryption.Encrypt(data);
			var commandId = commandHolder.GetCommand(payload.CommandType);

			return new Message {CommandId = commandId, Data = data};
		}

		public IPayload GetPayload(IMessage message)
		{
			var messageData = encryption.Decrypt(message.Data);
			messageData = compression.Decompress(messageData);

			var command = commandHolder.GetCommand(message.CommandId);
			var genericArgument = commandHolder.GetGenericArgument(command);

			using (var stream = new MemoryStream(messageData))
			{
				var data = serializer.Deserialize(stream, genericArgument);
				return new Payload {CommandType = command, Data = data};
			}
		}
	}
}