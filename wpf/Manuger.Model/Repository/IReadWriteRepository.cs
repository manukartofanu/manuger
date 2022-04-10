
namespace Manuger.Model.Repository
{
	public interface IReadWriteRepository<T> : IReadRepository<T>
		where T : class
	{
		void CreateItem(T item);
	}
}
