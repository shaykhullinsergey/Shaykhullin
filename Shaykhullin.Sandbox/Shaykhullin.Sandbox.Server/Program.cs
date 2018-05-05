using System;
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
		public async Task Execute(Command command)
		{
			Console.WriteLine(command.Message);
			await command.Connection.Send((int.Parse(command.Message) + 1).ToString()).To<Command>();
		}
	}
}
