namespace Network.Core
{
	internal class Configuration : IConfiguration
	{
		public Configuration(string host, int port)
		{
			Host = host;
			Port = port;
		}
		
		public string Host { get; }
		public int Port { get; }
	}
}