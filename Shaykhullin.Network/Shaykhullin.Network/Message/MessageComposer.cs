using System;
using Shaykhullin.ArrayPool;
using Shaykhullin.Serializer;
using Shaykhullin.DependencyInjection;
using Shaykhullin.Stream;

namespace Shaykhullin.Network.Core
{
	internal class MessageComposer : IMessageComposer
	{
		private readonly ISerializer serializer;
		private readonly ICompression compression;
		private readonly IEncryption encryption;
		private readonly ICommandHolder commandHolder;
		private readonly IArrayPool arrayPool;

		public MessageComposer(IContainer container)
		{
			serializer = container.Resolve<ISerializer>();
			compression = container.Resolve<ICompression>();
			encryption = container.Resolve<IEncryption>();
			commandHolder = container.Resolve<ICommandHolder>();
			arrayPool = container.Resolve<IArrayPool>();
		}

		public IMessage GetMessage(IPayload payload)
		{
			using (var stream = new MemoryStream(arrayPool))
			{
				var commandId = commandHolder.GetCommand(payload.CommandType);

				stream.WriteInt32(commandId);
				
				serializer.Serialize(stream, payload.Data);
				compression.Compress(stream);
				encryption.Encrypt(stream);

				return new Message
				{
					DataStreamBuffer = stream.Buffer,
					DataStreamLength = (int)stream.Length
				};
			}
		}

		public IPayload GetPayload(IMessage message)
		{
			using (var stream = new MemoryStream(message.DataStreamBuffer))
			{
				var commandId = stream.ReadInt32();

				var commandType = commandHolder.GetCommand(commandId);
				var genericArgument = commandHolder.GetGenericArgument(commandType);

				encryption.Decrypt(stream);
				compression.Decompress(stream);

				var data = serializer.Deserialize(stream, genericArgument);
				return new Payload
				{
					CommandType = commandType,
					Data = data
				};
			}
		}
	}
}
