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

		public Message GetMessage<TData>(IPayload<TData> payload)
		{
			using (var stream = new ValueStream(arrayPool))
			{
				var commandId = commandHolder.GetCommand(payload.CommandType);

				stream.WriteInt32(commandId);
				
				serializer.Serialize(stream, payload.Data);
				compression.Compress(stream);
				encryption.Encrypt(stream);

				return new Message(stream.Buffer, (int)stream.Length);
			}
		}

		public IPayload<TData> GetPayload<TData>(Message message)
		{
			using (var stream = new ValueStream(message.Data))
			{
				var commandId = stream.ReadInt32();

				var commandType = commandHolder.GetCommand(commandId);
				var genericArgument = commandHolder.GetGenericArgument(commandType);

				encryption.Decrypt(stream);
				compression.Decompress(stream);

				var data = serializer.Deserialize(stream, genericArgument);
				return new Payload<TData>
				{
					CommandType = commandType,
					Data = (TData)data
				};
			}
		}
	}
}
