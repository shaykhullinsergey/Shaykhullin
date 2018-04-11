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

			config.Match<ConnectInfo>()
				.From<Connect>()
				.With<ConnectHandler>();

			config.Match<Person>()
				.From<Event>()
				.With<Handler>();

			config.Match<DisconnectInfo>()
				.From<Disconnect>()
				.With<DisconnectHandler>();

			var client = config.Create("127.0.0.1", 4000);

			using (var connection = await client.Connect())
			{
				await connection.Send
				(
					new Person
					{
						Name = "Angel",
						Age = 32,
						Children = new Person[]
						{
							new Person
							{
								Name = "Bob",
								Age = 17,
								Children = new Person[0]
							},
							new Person
							{
								Name = "Sandra",
								Age = 14,
								Children = new Person[]
								{
									new Person
									{
										Name = "Mike",
										Age = 1,
										Children = null
									}
								}
							}
						}
					}
				).To<Event>();
				Thread.Sleep(1000);
			}
		}
	}

	class Person
	{
		public string Name { get; set; }
		public int Age { get; set; }
		public Person[] Children { get; set; }
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
}
