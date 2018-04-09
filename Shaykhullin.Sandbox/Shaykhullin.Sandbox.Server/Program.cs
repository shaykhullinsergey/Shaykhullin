using Network;
using System;
using System.Threading.Tasks;

namespace Shaykhullin.Sandbox.Server
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var config = new ServerConfig();

			config.When<Event>()
				.Use<Handler>();

			await config.Create("127.0.0.1", 4000).Run();
		}
	}

	class Event : IEvent<string>
	{
		public Event(IConnection connection, string message)
		{
			Connection = connection;
			Message = message;
		}

		public IConnection Connection { get; }
		public string Message { get; }
	}

	class Handler : IHandler<Event>
	{
		public async Task Execute(Event @event)
		{
			Console.WriteLine(@event.Message);
			await @event.Connection.Send("Data")
				.In<Event>();
		}
	}
}
