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

			config.On<int>().In<Command>().Call<Handler>();

			using (var app = config.Create("127.0.0.1", 4000))
			{
				await app.Run();
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
		public void Execute(Command command)
		{
			command.Connection.Send(command.Message + 1).To<Command>();
		}
	}
}
