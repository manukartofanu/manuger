using Manuger.Core.Model;
using Manuger.Core.Repository.Base;

namespace Manuger.Core.Repository
{
	public interface ITourRepository : IReadWriteRepository<Tour>
	{
		Tour[] GetToursInLeague(long leagueId);
		void InsertTours(Tour[] tours);
	}
}
