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

			config.On<string>().In<Command>().Call<Handler>();

			using (var app = config.Create("127.0.0.1", 4002))
			{
				await app.Run();
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

	class Handler : IHandler<string, Command>
	{
		public Task Execute(Command command)
		{
			Console.WriteLine(command.Message);
			return Task.CompletedTask;
		}
	}
}
