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

			config.Match<string>()
				.From<Event>()
				.With<Handler>();

			var client = config.Create("127.0.0.1", 4000);

			using (var connection = await client.Connect())
			{
				while (true)
				{
					await connection.Send(Console.ReadLine()).To<Event>();
				}
			}
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
			return Task.CompletedTask;
		}
	}
}
