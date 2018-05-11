using System;
using System.Collections.Generic;
using Shaykhullin.ArrayPool;

namespace Shaykhullin.Network.Core
{
	internal class PacketsComposer : IPacketsComposer
	{
		private const int PacketSize = 256;
		private const int HeaderSize = 3;
		private const int PayloadSize = PacketSize - HeaderSize;

		private int uniqueMessageId;
		private readonly IArrayPool arrayPool;

		public PacketsComposer(IArrayPool arrayPool)
		{
			this.arrayPool = arrayPool;
		}

		public Packet GetPacket(byte[] buffer)
		{
			return new Packet(buffer);
		}

		public byte[] GetBuffer()
		{
			return arrayPool.GetArrayExact<byte>(PacketSize);
		}

		public void ReleaseBuffer(byte[] buffer)
		{
			arrayPool.ReleaseArray(buffer);
		}

		public Packet[] GetPackets(Message message)
		{
			var id = (byte)(uniqueMessageId++ % byte.MaxValue);

			var data = message.Data;
			var count = GetPacketCount(message.Length);

			var packets = arrayPool.GetArrayExact<Packet>(count);
			
			for (var order = 0; order < count; order++)
			{
				var end = (order + 1) * PayloadSize >= message.Length;
				
				var length = (byte)(!end ? PayloadSize : message.Length - order * PayloadSize);
				
				var buffer = GetBuffer();

				buffer[0] = id;
				buffer[1] = length;
				buffer[2] = (byte)(end ? 1 : 0);
				
				Array.Copy(data, order * PayloadSize, buffer, HeaderSize, length);

				packets[order] = GetPacket(buffer);
			}

			return packets;
		}

		public Message GetMessage(IList<Packet> packets)
		{
			var dataLength = 0;
			for (var i = 0; i < packets.Count; i++)
			{
				dataLength += packets[i].Length;
			}

			var data = arrayPool.GetArray<byte>(dataLength);
			
			var position = 0;
			for (var i = 0; i < packets.Count; i++)
			{
				Array.Copy(packets[i].Buffer, HeaderSize, data, position, packets[i].Length);
				position += packets[i].Length;
			}

			return new Message(data, data.Length);
		}

		private static int GetPacketCount(int length) => length / PayloadSize + (length % PayloadSize == 0 ? 0 : 1);
	}
}