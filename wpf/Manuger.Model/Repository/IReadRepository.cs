using System;
using System.Collections.Generic;

namespace Manuger.Model.Repository
{
	public interface IReadRepository<T> : IDisposable
		where T : class
	{
		IEnumerable<T> GetAllItems();
		T GetItem(long id);
	}
}
