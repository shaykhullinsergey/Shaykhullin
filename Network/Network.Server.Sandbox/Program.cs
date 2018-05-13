using Network.Core;

namespace Network.Server.Sandbox
{
	public class MyChannel : IChannel
	{
	}

	public class MyCommand : ICommand<int>
	{
		public MyCommand(Connection connection, int data)
		{
			Connection = connection;
			Data = data;
		}

		public Connection Connection { get; }
		public int Data { get; }
	}

	public class MyHandler : IHandler<int, MyCommand>
	{
		public void Execute(in MyCommand command)
		{
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			using (var config = new ServerConfig(1))
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
						
						when.From<MyCommand>()
							.Call<MyHandler>();
					}
				}

				using (var server = config.Create())
				{
					// server.Listen("127.0.0.1", 4000);
				}
			}
		}
	}
}