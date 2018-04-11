namespace Shaykhullin.Serializer.Core
{
	public interface IUseBuilder<TData>
	{
		void With<TConverter>()
			where TConverter : IConverter<TData>;
	}
}
