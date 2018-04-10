using Shaykhullin.Network;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shaykhullin.Sandbox.Client
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var config = new ClientConfig();

			config.When<ConnectInfo>()
				.From<Connect>()
				.Call<ConnectHandler>();

			config.When<int>()
				.From<Event>()
				.Call<Handler>();

			config.When<DisconnectInfo>()
				.From<Disconnect>()
				.Call<DisconnectHandler>();

			var client = config.Create("127.0.0.1", 4000);

			using (var connection = await client.Connect())
			{
				await connection.Send(1).To<Event>();
				Thread.Sleep(1000);
			}
		}
	}

	class DisconnectHandler : IHandler<DisconnectInfo, Disconnect>
	{
		public Task Execute(Disconnect @event)
		{
			Console.WriteLine("DISCONNECT" + @event.Message.Reason);
			return Task.CompletedTask;
		}
	}

	struct ConnectHandler : IHandler<ConnectInfo, Connect>
	{
		public Task Execute(Connect @event)
		{
			Console.WriteLine("CONNECT");
			return Task.CompletedTask;
		}
	}

	struct Event : IEvent<int>
	{
		public Event(IConnection connection, int message)
		{
			Connection = connection;
			Message = message;
		}

		public IConnection Connection { get; }
		public int Message { get; }
	}

	struct Handler : IHandler<int, Event>
	{
		public async Task Execute(Event @event)
		{
			Console.WriteLine(@event.Message);
			var value = @event.Message + 1;
			await @event.Connection.Send(value).To<Event>();
		}
	}
}
