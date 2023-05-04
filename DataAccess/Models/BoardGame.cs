using LendStuff.DataAccess.Models;

namespace LendStuff.Server.Models;

public class BoardGame
{
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public string Title { get; set; }
	public int? ReleaseYear { get; set; }
	public string Description { get; set; }
	public int Condition { get; set; }
	public string? Comment { get; set; }
	public bool Available { get; set; }
	public List<Genre>? Genres { get; set; } = new ();

	public string? BggLink { get; set; }
	public ICollection<ApplicationUser> Users { get; set; }
}