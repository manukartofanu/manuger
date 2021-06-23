using System;
using System.Collections.Generic;

namespace Manuger.Core.Database
{
	public interface IRepository<T> : IDisposable
		where T : class
	{
		IEnumerable<T> GetAllItems();
		T GetItem(long id);
		void CreateItem(T item);
	}
}
