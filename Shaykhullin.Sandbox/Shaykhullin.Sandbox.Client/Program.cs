using System;
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

			config.On<int>().In<Command>().Call<Handler>();
			
			using (var app = config.Create("127.0.0.1", 4002))
			{
				using (var connection = await app.Connect())
				{
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
				GC.CollectionCount(0);
				GC.CollectionCount(1);
				GC.CollectionCount(2);
			}
			
			await command.Connection.Send(command.Message + 1).To<Command>();
		}
	}
}