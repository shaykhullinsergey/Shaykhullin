using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shaykhullin.Network.Core
{
	internal class PacketsComposer : IPacketsComposer
	{
		private const int PacketSize = 256;
		private const int HeaderSize = 5;
		private const int PayloadSize = PacketSize - HeaderSize;

		private int uniqueMessageId;
		private readonly ICommandRaiser commandRaiser;

		public PacketsComposer(ICommandRaiser commandRaiser)
		{
			this.commandRaiser = commandRaiser;
		}

		public IPacket GetPacket(byte[] data)
		{
			var chunk = GetBuffer();
			Array.Copy(data, HeaderSize, chunk, 0, PayloadSize);

			var order = new ByteUnion(data[1], data[2]).UInt16;
			
			return new Packet
			{
				Id = data[0],
				Order = order,
				Length = data[3],
				IsLast = data[4] == 1,
				Chunk = chunk
			};
		}

		public byte[] GetBytes(IPacket packet)
		{
			var data = GetBuffer();
			data[0] = packet.Id;
			
			var union = new ByteUnion(packet.Order);
			data[1] = union.Byte1;
			data[2] = union.Byte2;
			
			data[3] = packet.Length;
			data[4] = (byte)(packet.IsLast ? 1 : 0);
			
			Array.Copy(packet.Chunk, 0, data, HeaderSize, PayloadSize);
			return data;
		}

		public byte[] GetBuffer()
		{
			return new byte[PacketSize];
		}

		public async Task<IPacket[]> GetPackets(IMessage message)
		{
			var id = (byte)(uniqueMessageId++ % byte.MaxValue);

			var data = new byte[message.Data.Length + 4];
			
			var union = new ByteUnion(message.CommandId);
			data[0] = union.Byte1;
			data[1] = union.Byte2;
			data[2] = union.Byte3;
			data[3] = union.Byte4;
			
			Array.Copy(message.Data, 0, data, 4, message.Data.Length);

			var count = GetPacketCount(data.Length);

			if (count > ushort.MaxValue)
			{
				await commandRaiser.RaiseCommand(new ErrorPayload("Message size is too long")).ConfigureAwait(false);

				throw new InvalidOperationException();
			}

			var packets = new IPacket[count];

			for (var order = 0; order < count; order++)
			{
				var end = (order + 1) * PayloadSize >= data.Length;
				var length = (byte)(!end ? PayloadSize : data.Length - order * PayloadSize);
				var chunk = new byte[PayloadSize];

				Array.Copy(data, order * PayloadSize, chunk, 0, length);

				packets[order] = new Packet
				{
					Id = id,
					Order = (ushort)(order + 1),
					Length = length,
					IsLast = end,
					Chunk = chunk
				};
			}

			return packets;
		}

		public IMessage GetMessage(IList<IPacket> packets)
		{
			var chunk = packets[0].Chunk;
			
			var union = new ByteUnion(chunk[0], chunk[1], chunk[2], chunk[3]);
			var commandId = union.Int32;

			var dataLength = 0;
			for (var i = 0; i < packets.Count; i++)
			{
				dataLength += packets[i].Length;
			}

			var data = new byte[dataLength - 4];
			var firstPacketLength = packets[0].Length - 4;
			Array.Copy(packets[0].Chunk, 4, data, 0, firstPacketLength);
			var position = firstPacketLength;

			for (var i = 1; i < packets.Count; i++)
			{
				chunk = packets[i].Chunk;
				Array.Copy(packets[i].Chunk, 0, data, position, packets[i].Length);
				position += chunk.Length;
			}
			
			// TODO: Performace!!
//			var data2 = packets.SelectMany(x => x.Chunk.Take(x.Length))
//				.Skip(4)
//				.ToArray();
			
			return new Message { CommandId = commandId, Data = data };
		}

		private static int GetPacketCount(int length) => length / PayloadSize + (length % PayloadSize == 0 ? 0 : 1);
	}
}