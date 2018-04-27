namespace Shaykhullin.Network
{
	public interface ICommand<out TData>
	{
		IConnection Connection { get; }
		TData Message { get; }
	}
}