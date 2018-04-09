namespace Shaykhullin.Serializer.Core
{

	public interface IUseBuilder<TData>
	{
		void Use<TConverter>()
			where TConverter : IConverter<TData>;
	}
}
