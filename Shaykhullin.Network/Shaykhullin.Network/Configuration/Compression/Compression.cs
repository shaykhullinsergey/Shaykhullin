namespace Network.Core
{
  internal class Compression : ICompression
  {
    public byte[] Compress(byte[] data)
    {
      return data;
    }

    public byte[] Decompress(byte[] data)
    {
      return data;
    }
  }
}
