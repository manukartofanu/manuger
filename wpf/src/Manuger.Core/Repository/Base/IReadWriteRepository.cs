
namespace Manuger.Core.Repository.Base
{
	public interface IReadWriteRepository<T> : IReadRepository<T>
		where T : class
	{
		void CreateItem(T item);
	}
}
