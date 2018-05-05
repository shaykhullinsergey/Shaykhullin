using System.Runtime.InteropServices;

namespace Shaykhullin
{
	[StructLayout(LayoutKind.Explicit)]
	public readonly struct UnifiedUnion
	{
		[FieldOffset(0)] public readonly byte Byte1;
		[FieldOffset(1)] public readonly byte Byte2;
		[FieldOffset(2)] public readonly byte Byte3;
		[FieldOffset(3)] public readonly byte Byte4;
		[FieldOffset(4)] public readonly byte Byte5;
		[FieldOffset(5)] public readonly byte Byte6;
		[FieldOffset(6)] public readonly byte Byte7;
		[FieldOffset(7)] public readonly byte Byte8;
		
		[FieldOffset(0)] public readonly short Int16;
		[FieldOffset(0)] public readonly ushort UInt16;
		
		[FieldOffset(0)] public readonly int Int32;
		[FieldOffset(0)] public readonly uint UInt32;
		
		[FieldOffset(0)] public readonly float Single;
		
		[FieldOffset(0)] public readonly long Int64;
		[FieldOffset(0)] public readonly ulong UInt64;
		
		[FieldOffset(0)] public readonly double Double;

		public UnifiedUnion(byte byte1, byte byte2) : this()
		{
			Byte1 = byte1;
			Byte2 = byte2;
		}

		public UnifiedUnion(byte byte1, byte byte2, byte byte3, byte byte4) : this()
		{
			Byte1 = byte1;
			Byte2 = byte2;
			Byte3 = byte3;
			Byte4 = byte4;
		}

		public UnifiedUnion(byte byte1, byte byte2, byte byte3, byte byte4, byte byte5, byte byte6, byte byte7, byte byte8) : this()
		{
			Byte1 = byte1;
			Byte2 = byte2;
			Byte3 = byte3;
			Byte4 = byte4;
			Byte5 = byte5;
			Byte6 = byte6;
			Byte7 = byte7;
			Byte8 = byte8;
		}

		public UnifiedUnion(short int16) : this()
		{
			Int16 = int16;
		}

		public UnifiedUnion(ushort uInt16) : this()
		{
			UInt16 = uInt16;
		}

		public UnifiedUnion(int int32) : this()
		{
			Int32 = int32;
		}

		public UnifiedUnion(float single) : this()
		{
			Single = single;
		}

		public UnifiedUnion(double d) : this()
		{
			Double = d;
		}

		public UnifiedUnion(uint uInt32) : this()
		{
			UInt32 = uInt32;
		}

		public UnifiedUnion(ulong uInt64) : this()
		{
			UInt64 = uInt64;
		}

		public UnifiedUnion(long int64) : this()
		{
			Int64 = int64;
		}
	}
}