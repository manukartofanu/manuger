
namespace Manuger.Model
{
	public class Team : IIdentable
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int CountryId { get; set; }
	}
}
