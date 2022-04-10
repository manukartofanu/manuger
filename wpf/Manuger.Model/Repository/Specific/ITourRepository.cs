
namespace Manuger.Model.Repository.Specific
{
	public interface ITourRepository : IReadWriteRepository<Tour>
	{
		Tour[] GetToursInLeague(long leagueId);
		void InsertTours(Tour[] tours);
	}
}
