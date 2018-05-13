using System.Threading;

namespace Network.Client.Sandbox
{
	public struct MyChannel : Core.IChannel
	{
	}

	public struct MyCommand : ICommand<int>
	{
		public MyCommand(Connection connection, int data)
		{
			Connection = connection;
			Data = data;
		}

		public Connection Connection { get; }
		public int Data { get; }
	}

	public struct MyHandler : IHandler<int, MyCommand>
	{
		public void Execute(in MyCommand command)
		{
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			using (var config = new ClientConfig(1))
			{
				config.When<int>()
					.From<MyCommand>()
					.Call<MyHandler>();

				config.Channel<MyChannel>()
					.When<int>()
					.From<MyCommand>()
					.Call<MyHandler>();

				using (var channel = config.Channel<MyChannel>())
				{
					channel.When<int>()
						.From<MyCommand>()
						.Call<MyHandler>();

					channel.When<int>()
						.From<MyCommand>()
						.Call<MyHandler>();

					using (var inner = channel.Channel<MyChannel>())
					{
						inner.When<int>()
							.From<MyCommand>()
							.Call<MyHandler>();

						inner.When<int>()
							.From<MyCommand>()
							.Call<MyHandler>();
					}

					using (var when = channel.When<int>())
					{
						when.From<MyCommand>()
							.Call<MyHandler>();
					}
				}

				using (var client = config.Create())
				{
					using (var connection = client.Connect("127.0.0.1", 4000))
					{
						connection.Send(12).To<MyCommand>();
						Thread.Sleep(-1);
					}
				}
			}
		}
	}
}