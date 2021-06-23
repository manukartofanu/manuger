using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manuger.Core.Database
{
	public interface ILeagueRepository
	{
		League[] GetLeagues();
		long InsertLeague(League league);
		void InsertTeamsIntoLeague(long leagueId, Team[] teams);
	}
}
