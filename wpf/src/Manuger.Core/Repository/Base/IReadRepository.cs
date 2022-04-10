using System;
using System.Collections.Generic;

namespace Manuger.Core.Repository.Base
{
	public interface IReadRepository<T> : IDisposable
		where T : class
	{
		IEnumerable<T> GetAllItems();
		T GetItem(long id);
	}
}
