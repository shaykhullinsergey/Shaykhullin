using System.IO;

namespace Shaykhullin.Serializer.Tests
{
	public class SerializerTests
	{
		public Stream CreateStream()
		{
			return new MemoryStream();
		}
	}
}
