
namespace Manuger.Core
{
	public class Game
	{
		public int Id { get; set; }
		public int TourId { get; set; }
		public int HomeTeamId { get; set; }
		public int AwayTeamId { get; set; }
		public int? HomeGoals { get; set; }
		public int? AwayGoals { get; set; }
		public bool IsFinished { get; set; }
	}
}
