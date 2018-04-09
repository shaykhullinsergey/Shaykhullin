using Shaykhullin.Serializer;
using System.IO;

namespace Shaykhullin.Sandbox.Serializer
{
	class Program
	{
		static void Main(string[] args)
		{
			var test = new Test
			{
				MyProperty1 = 11,
				MyProperty2 = new Test
				{
					MyProperty1 = 22,
					MyProperty3 = 33
				},
				MyProperty3 = 44
			};

			var config = new SerializerConfig();

			var s = config.Create();
			using (var stream = new MemoryStream())
			{
				s.Serialize<byte>(stream, 252);
				stream.Position = 0;
				var t = s.Deserialize<byte>(stream);
			}
		}
	}

	class Test
	{
		public int MyProperty1 { get; set; }
		public Test MyProperty2 { get; set; }
		public int MyProperty3 { get; set; }
	}
}
