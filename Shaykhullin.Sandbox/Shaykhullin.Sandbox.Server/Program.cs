using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Shaykhullin.Network;

namespace Shaykhullin.Sandbox.Server
{
	class Program
	{
		static async Task Main()
		{
			var config = new ServerConfig();

			config.On<int>().In<Command>().Call<Handler>();

			using (var app = config.Create("127.0.0.1", 4002))
			{
				await app.Run();
			}
		}
	}

	struct Command : ICommand<int>
	{
		public Command(IConnection connection, int message)
		{
			Connection = connection;
			Message = message;
		}

		public IConnection Connection { get; }
		public int Message { get; }
	}

	struct Handler : IHandler<int, Command>
	{
		public async Task Execute(Command command)
		{
			Console.WriteLine(command.Message);
			await command.Connection.Send(command.Message + 1).To<Command>();

			if (command.Message > 30000)
			{
				Console.WriteLine();
			}
		}
	}
}
