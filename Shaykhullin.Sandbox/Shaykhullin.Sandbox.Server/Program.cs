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

			config.Match<string>()
				.From<Event>()
				.With<Handler>();

			Console.WriteLine("Started");
			await config.Create("127.0.0.1", 4002).Run();
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
		public async Task Execute(Event @event)
		{
			Console.WriteLine(@event.Message);
			await @event.Connection.Send("Echo: " + @event.Message).To<Event>();
		}
	}
}
