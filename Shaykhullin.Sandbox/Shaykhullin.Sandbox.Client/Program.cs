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

			await connection.Send(new IntHolder { MyProperty = 11}).In<Event>();
			Thread.Sleep(-1);
		}
	}

	class IntHolder
	{
		public int MyProperty { get; set; }
	}

	class Event : IEvent<IntHolder>
	{
		public Event(IConnection connection, IntHolder message)
		{
			Connection = connection;
			Message = message;
		}

		public IConnection Connection { get; }
		public IntHolder Message { get; }
	}

	class Handler : IHandler<Event>
	{
		public async Task Execute(Event @event)
		{
			Console.WriteLine(@event.Message.MyProperty);
			@event.Message.MyProperty++;
			await @event.Connection.Send(@event.Message)
				.In<Event>();
		}
	}
}
