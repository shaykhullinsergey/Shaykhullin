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

				var union = new ByteUnion(commandId);
				stream.WriteByte(union.Byte1);
				stream.WriteByte(union.Byte2);
				stream.WriteByte(union.Byte3);
				stream.WriteByte(union.Byte4);

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
				var commandId = new ByteUnion(
					(byte)stream.ReadByte(),
					(byte)stream.ReadByte(),
					(byte)stream.ReadByte(),
					(byte)stream.ReadByte()).Int32;

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
