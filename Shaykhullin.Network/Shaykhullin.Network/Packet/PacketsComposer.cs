using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shaykhullin.Network.Core
{
	internal class PacketsComposer : IPacketsComposer
	{
		private static int UniqueId;
		private const int PacketSize = 256;
		private const int HeaderSize = 5;
		private const int PayloadSize = PacketSize - HeaderSize;

		private readonly IEventRaiser eventRaiser;

		public PacketsComposer(IEventRaiser eventRaiser)
		{
			this.eventRaiser = eventRaiser;
		}

		public async Task<IPacket> GetPacket(byte[] data)
		{
			var chunk = await GetBuffer().ConfigureAwait(false);
			Array.Copy(data, HeaderSize, chunk, 0, PayloadSize);

			return new Packet
			{
				Id = data[0],
				Order = BitConverter.ToUInt16(data, 1),
				Length = data[3],
				End = data[4] == 1,
				Chunk = chunk
			};
		}

		public async Task<byte[]> GetBytes(IPacket packet)
		{
			var data = await GetBuffer().ConfigureAwait(false);

			data[0] = packet.Id;
			Array.Copy(BitConverter.GetBytes(packet.Order), 0, data, 1, 2);
			data[3] = packet.Length;
			data[4] = (byte)(packet.End ? 1 : 0);
			Array.Copy(packet.Chunk, 0, data, HeaderSize, PayloadSize);
			return data;
		}

		public async Task<byte[]> GetBuffer()
		{
			return await Task.FromResult(new byte[PacketSize]).ConfigureAwait(false);
		}

		public async Task<IPacket[]> GetPackets(IMessage message)
		{
			var id = (byte)(UniqueId++ % byte.MaxValue);

			var data = new byte[message.Data.Length + 4];
			Array.Copy(BitConverter.GetBytes(message.EventId), data, 4);
			Array.Copy(message.Data, 0, data, 4, message.Data.Length);

			var count = GetPacketCount(data.Length);

			if (count > ushort.MaxValue)
			{
				await eventRaiser.Raise(new ErrorPayload("Message size is too long")).ConfigureAwait(false);

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
					End = end,
					Chunk = chunk
				};
			}

			return await Task.FromResult(packets).ConfigureAwait(false);
		}

		public async Task<IMessage> GetMessage(IList<IPacket> packets)
		{
			// TODO: Performace!!
			var data = packets.SelectMany(x => x.Chunk.Take(x.Length));

			var eventId = BitConverter.ToInt32(data.Take(4).ToArray(), 0);

			return await Task.FromResult(
				(IMessage)new Message { EventId = eventId, Data = data.Skip(4).ToArray() })
					.ConfigureAwait(false);
		}

		private static int GetPacketCount(int length) => length / PayloadSize + (length % PayloadSize == 0 ? 0 : 1);
	}
}