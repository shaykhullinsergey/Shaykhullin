using Shaykhullin.Serializer;
using System.IO;

namespace Shaykhullin.Sandbox.Serializer
{
	class Program
	{
		static void Main(string[] args)
		{
			Test[] test = new Test[] { new Test3 { Prop = true, Prop2 = 12 }, new Test { Prop = false }, new Test3 { Prop = true, Prop2 = int.MaxValue, Prop3 = "MaxValuesTing"} };

			var config = new SerializerConfig();

			config.Match<Test>();
			config.Match<Test3>();

			var serializer = config.Create();

			using (var stream = new MemoryStream())
			{
				serializer.Serialize(stream, test);
				stream.Position = 0;
				var instance = serializer.Deserialize<Test[]>(stream);
			}
		}
	}

	class Test
	{
		public bool Prop { get; set; }
	}

	class Test2 : Test
	{
		public int Prop2 { get; set; }
	}

	class Test3 : Test2
	{
		public string Prop3 { get; set; }
	}
}
