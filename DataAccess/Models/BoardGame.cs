using LendStuff.DataAccess.Models;

namespace LendStuff.Server.Models;

public class BoardGame
{
	public string Id { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public int ReleaseYear { get; set; }
	public string Description { get; set; } = string.Empty;
	public int Condition { get; set; }
	public string? Comment { get; set; } = string.Empty;
	public bool Available { get; set; }
	public List<Genre>? Genres { get; set; } = new ();

	public string? BggLink { get; set; } = string.Empty;
	public ICollection<ApplicationUser> Users { get; set; } //Behövs denna vara med här?
}