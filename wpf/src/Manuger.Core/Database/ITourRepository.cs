
namespace Manuger.Core.Database
{
	public interface ITourRepository : IRepository<Tour>
	{
		Tour[] GetToursInLeague(long leagueId);
		void InsertTours(Tour[] tours);
	}
}
