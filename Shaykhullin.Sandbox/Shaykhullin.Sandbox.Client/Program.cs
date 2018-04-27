using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Shaykhullin.Network;

namespace Shaykhullin.Sandbox.Client
{
	class Program
	{
		public static Stopwatch sw = new Stopwatch();
		
		static async Task Main()
		{
 			var config = new ClientConfig();

			config.On<int>().In<Command>().Call<Handler>();

			using (var app = config.Create("127.0.0.1", 4002))
			{
				using (var connection = await app.Connect())
				{
					sw.Start();
					await connection.Send(1).To<Command>();
					Thread.Sleep(-1);
				}
			}
		}
	}

	class Command : ICommand<int>
	{
		public Command(IConnection connection, int message)
		{
			Connection = connection;
			Message = message;
		}

		public IConnection Connection { get; }
		public int Message { get; }
	}

	class Handler : IHandler<int, Command>
	{
		public async Task Execute(Command command)
		{
			Console.WriteLine(command.Message);

			if (command.Message > 30000)
			{
				Program.sw.Stop();
				Console.WriteLine(Program.sw.ElapsedMilliseconds);
				return;
			}
			
			await command.Connection.Send(command.Message + 1).To<Command>();
		}
	}
}