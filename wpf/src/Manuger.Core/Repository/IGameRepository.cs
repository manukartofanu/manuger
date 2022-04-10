using Manuger.Core.Model;
using Manuger.Core.Repository.Base;

namespace Manuger.Core.Repository
{
	public interface IGameRepository : IReadWriteRepository<Game>
	{
		Game[] GetGamesInTour(int tourId);
		Game[] GetGamesFinished(int leagueId);
		void InsertGames(Game[] games);
		void UpdateGames(Game[] games);
	}
}
