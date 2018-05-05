using Shaykhullin.Stream;

namespace Shaykhullin.Serializer.Tests
{
	public class SerializerTests
	{
		public System.IO.Stream CreateStream()
		{
			return new MemoryStream();
		}
	}
}
