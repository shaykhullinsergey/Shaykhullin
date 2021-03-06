﻿using System.IO;
using Shaykhullin.Stream;

namespace Shaykhullin.Serializer.Core
{
	internal class UInt32Converter : Converter<uint>
	{
		public override void Serialize(ValueStream stream, uint data)
		{
			var union = new UnifiedUnion(data);
			stream.WriteByte(union.Byte1);
			stream.WriteByte(union.Byte2);
			stream.WriteByte(union.Byte3);
			stream.WriteByte(union.Byte4);
		}

		public override uint Deserialize(ValueStream stream)
		{
			return new UnifiedUnion(
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte()).UInt32;
		}
	}
}
