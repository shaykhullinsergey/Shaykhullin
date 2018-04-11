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

			config.Match<ConnectInfo>()
				.From<Connect>()
				.With<ConnectHandler>();

			config.Match<Person>()
				.From<Event>()
				.With<Handler>();

			config.Match<DisconnectInfo>()
				.From<Disconnect>()
				.With<DisconnectHandler>();

			await config.Create("127.0.0.1", 4000).Run();
		}
	}

	class Person
	{
		public string Name { get; set; }
		public int Age { get; set; }
		public Person[] Children { get; set; }

		public override string ToString()
		{
			var children = Children?.Select(x => x.ToString()) ?? Enumerable.Empty<string>();

			return $"Name: {Name}, Age: {Age}, Children: [{string.Join(",", children)}]";
		}
	}

	struct Event : IEvent<Person>
	{
		public Event(IConnection connection, Person message)
		{
			Connection = connection;
			Message = message;
		}

		public IConnection Connection { get; }
		public Person Message { get; }
	}

	struct Handler : IHandler<Person, Event>
	{
		public Task Execute(Event @event)
		{
			Console.WriteLine(@event.Message);
			return Task.CompletedTask;
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
