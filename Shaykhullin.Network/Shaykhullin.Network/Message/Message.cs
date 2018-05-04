using System.IO;

namespace Shaykhullin.Network.Core
{
	internal class Message : IMessage
	{
		public byte[] DataStreamBuffer { get; set; }
		public int DataStreamLength { get; set; }
	}
}