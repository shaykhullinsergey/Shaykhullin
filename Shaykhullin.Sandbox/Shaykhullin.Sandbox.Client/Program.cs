using Network;
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

			config.When<Event>()
				.Use<Handler>();

			var connection = await config.Create("127.0.0.1", 4000)
				.Connect();

			await connection.Send("DATA").In<Event>();
			Thread.Sleep(-1);
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
