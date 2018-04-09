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

			config.When<int>()
				.From<Event>()
				.Call<Handler>();

			await config.Create("127.0.0.1", 4000).Run();
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
			var m = @event.Message + 1;
			await @event.Connection.Send(@event.Message).To<Event>();
		}
	}
}
