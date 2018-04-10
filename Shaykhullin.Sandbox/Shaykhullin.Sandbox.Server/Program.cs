using Shaykhullin.Network;
using System;
using System.Threading.Tasks;

namespace Shaykhullin.Sandbox.Server
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var config = new ServerConfig();

			config.When<ConnectInfo>()
				.From<Connect>()
				.Call<ConnectHandler>();

			config.When<int>()
				.From<Event>()
				.Call<Handler>();

			config.When<DisconnectInfo>()
				.From<Disconnect>()
				.Call<DisconnectHandler>();

			await config.Create("127.0.0.1", 4000).Run();
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
		public Task Execute(Event @event)
		{
			Console.WriteLine(@event.Message);
			return Task.CompletedTask;
		}
	}
}
