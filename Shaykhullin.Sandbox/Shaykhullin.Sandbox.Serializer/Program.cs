﻿using System.IO;
using Shaykhullin.Serializer;
using Shaykhullin.Stream;

namespace Shaykhullin.Sandbox.Serializer
{
	class Program
	{
		static void Main(string[] args)
		{
			Test[] test = new Test[] { new Test3 { Prop = false, Prop2 = 123 }, new Test2 { Prop = false, Prop2 = 12}, new Test3() };

			var config = new SerializerConfig();

			config.Match<Test>();
			config.Match<Test3>();

			var serializer = config.Create();

			using (var stream = new ValueStream())
			{
				serializer.Serialize(stream, test);
				stream.Seek(0);
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
