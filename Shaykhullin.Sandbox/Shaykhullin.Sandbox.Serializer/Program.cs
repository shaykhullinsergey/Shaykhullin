using Shaykhullin.Serializer;
using System.IO;

namespace Shaykhullin.Sandbox.Serializer
{
	class Program
	{
		static void Main(string[] args)
		{
			var test = new Test[] { new Test { Prop = true }, new Test { Prop = false }, new Test { Prop = true }, };

			var config = new SerializerConfig();

			var s = config.Create();
			using (var stream = new MemoryStream())
			{
				s.Serialize(stream, test);
				stream.Position = 0;
				var t = s.Deserialize<Test[]>(stream);
			}
		}
	}

	class Test
	{
		public bool Prop { get; set; }
	}
}
