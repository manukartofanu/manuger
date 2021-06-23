using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manuger.Core.Database
{
	public interface IGameRepository : IRepository<Game>
	{
		Game[] GetGamesInTour(int tourId);
		Game[] GetGamesFinished(int leagueId);
		void InsertGames(Game[] games);
		void UpdateGames(Game[] games);
	}
}
