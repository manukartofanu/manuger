using System;
using System.Collections.Generic;

namespace Manuger.Core.Database
{
	public interface IRepository<T> : IReadRepository<T>
		where T : class
	{
		void CreateItem(T item);
	}
}
