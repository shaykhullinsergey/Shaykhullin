using System.Runtime.InteropServices;

namespace Shaykhullin
{
	[StructLayout(LayoutKind.Explicit)]
	public readonly struct ByteConverter
	{
		[FieldOffset(0)] public readonly byte Byte1;
		[FieldOffset(1)] public readonly byte Byte2;
		[FieldOffset(2)] public readonly byte Byte3;
		[FieldOffset(3)] public readonly byte Byte4;
		[FieldOffset(0)] public readonly ushort UInt16;
		[FieldOffset(0)] public readonly int Int32;

		public ByteConverter(byte byte1, byte byte2) : this()
		{
			Byte1 = byte1;
			Byte2 = byte2;
		}

		public ByteConverter(byte byte1, byte byte2, byte byte3, byte byte4) : this()
		{
			Byte1 = byte1;
			Byte2 = byte2;
			Byte3 = byte3;
			Byte4 = byte4;
		}

		public ByteConverter(ushort uInt16) : this()
		{
			UInt16 = uInt16;
		}

		public ByteConverter(int int32) : this()
		{
			Int32 = int32;
		}
	}
}