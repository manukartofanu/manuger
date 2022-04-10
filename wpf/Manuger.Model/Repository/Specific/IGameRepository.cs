
namespace Manuger.Model.Repository.Specific
{
	public interface IGameRepository : IReadWriteRepository<Game>
	{
		Game[] GetGamesInTour(int tourId);
		Game[] GetGamesFinished(int leagueId);
		void InsertGames(Game[] games);
		void UpdateGames(Game[] games);
	}
}
