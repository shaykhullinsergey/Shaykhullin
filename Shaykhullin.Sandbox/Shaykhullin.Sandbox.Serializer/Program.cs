using Shaykhullin.Serializer;
using System.IO;

namespace Shaykhullin.Sandbox.Serializer
{
	class Program
	{
		static void Main(string[] args)
		{
			var test = new Test[] { new Test { Prop = 12, TestIn = new Test { Prop = 1313 } }, new Test { Prop = 13 } };

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
		public Test TestIn { get; set; }
	}
}
