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
			
			using (var app = config.Create("127.0.0.1", 4001))
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

			if (command.Message == 10000)
			{
				
			}

			if (20000 == command.Message)
			{
				
			}
			
			if (command.Message > 30000)
			{
				var g0 = GC.CollectionCount(0);
				var g1 = GC.CollectionCount(1);
				var g2 = GC.CollectionCount(2);
				return;
			}
			
			await command.Connection.Send(command.Message + 1).To<Command>();
		}
	}
}