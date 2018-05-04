using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Shaykhullin.Network;

namespace Shaykhullin.Sandbox.Client
{
	class Program
	{
		static async Task Main()
		{
 			var config = new ClientConfig();

			using (var app = config.Create("127.0.0.1", 4002))
			{
				using (var connection = await app.Connect())
				{
					for (var i = 0; i < 30000; i++)
					{
						await connection.Send("12345123451234512345123451234512345123451234512345123451234512345123451234512345123451234512345123451234512345123451234512345"
						+ "12345123451234512345123451234512345123451234512345123451234512345123451234512345123451234512345123451234512345123451234512345"
							+ "12345123451234512345123451234512345123451234512345123451234512345123451234512345123451234512345123451234512345123451234512345").To<Command>();
					}
					
					Thread.Sleep(5000);
					var p1 = GC.CollectionCount(0);
					var p2 = GC.CollectionCount(1);
					var p3 = GC.CollectionCount(2);

					Console.WriteLine(p1);
					Console.WriteLine(p2);
					Console.WriteLine(p3);
					Thread.Sleep(-1);
				}
			}
		}
	}

	class Command : ICommand<string>
	{
		public Command(IConnection connection, string message)
		{
			Connection = connection;
			Message = message;
		}

		public IConnection Connection { get; }
		public string Message { get; }
	}
}