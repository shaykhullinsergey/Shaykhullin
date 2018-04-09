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

			config.When<int>()
				.From<Event>()
				.Call<Handler>();

			var connection = await config.Create("127.0.0.1", 4000)
				.Connect();

			await connection.Send(1).To<Event>();
			Thread.Sleep(-1);
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
