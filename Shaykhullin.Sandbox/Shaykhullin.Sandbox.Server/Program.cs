using Shaykhullin.Network;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Shaykhullin.Sandbox.Server
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var config = new ServerConfig();

			config.Match<string>()
				.From<Event>()
				.With<Handler>();

			await config.Create("127.0.0.1", 4000).Run();
		}
	}

	struct Event : IEvent<string>
	{
		public Event(IConnection connection, string message)
		{
			Connection = connection;
			Message = message;
		}

		public IConnection Connection { get; }
		public string Message { get; }
	}

	struct Handler : IHandler<string, Event>
	{
		public Task Execute(Event @event)
		{
			Console.WriteLine(@event.Message);
			return @event.Connection.Send("Echo: " + @event.Message).To<Event>();
		}
	}

	class DisconnectHandler : IHandler<DisconnectInfo, Disconnect>
	{
		public Task Execute(Disconnect @event)
		{
			Console.WriteLine(@event.Message.Reason);
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
}
