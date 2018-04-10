using Shaykhullin.Serializer;
using System.IO;

namespace Shaykhullin.Sandbox.Serializer
{
	class Program
	{
		static void Main(string[] args)
		{
			Test[] test = { new Test { Prop = 11, Testt = new Test { Prop = 323 } }, new Test { Prop = 22 } };

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
		public int Prop { get; set; }
		public Test Testt { get; set; }
	}
}
