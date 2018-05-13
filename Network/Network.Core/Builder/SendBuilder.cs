namespace Network.Core
{
	public readonly struct SendBuilder<TData>
	{
		private readonly TData data;
		private readonly Transport transport;

		internal SendBuilder(Transport transport, TData data)
		{
			this.data = data;
			this.transport = transport;
		}
		
		public void To<TCommand>()
			where TCommand : ICommand<TData>
		{
			var buffer = new byte[256];
			buffer[0] = 12;
			transport.Send(buffer);
		}
	}
}