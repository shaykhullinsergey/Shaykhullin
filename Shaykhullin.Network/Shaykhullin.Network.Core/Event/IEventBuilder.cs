namespace Shaykhullin.Network.Core
{
	public interface IDataBuilder
	{
		ICommandBuilder<TData> On<TData>();
	}
}
