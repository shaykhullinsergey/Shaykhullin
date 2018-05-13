namespace Network.Core
{
	public readonly struct HandlerBuilder<TData, TCommand> 
		where TCommand : ICommand<TData>
	{
		private readonly HandlerConfiguration configuration;
		
		internal HandlerBuilder(HandlerConfiguration configuration)
		{
			this.configuration = configuration;
		}

		public void Call<THandler>()
			where THandler : IHandler<TData, TCommand>
		{
			configuration.EnsureHandler(typeof(THandler));
		}
	}
}