namespace Network
{
	public interface ICommand<TData>
	{
		Connection Connection { get; }
		TData Data { get; }
	}
}