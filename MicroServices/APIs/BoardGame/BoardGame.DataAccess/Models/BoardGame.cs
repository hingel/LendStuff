using System.ComponentModel.DataAnnotations;

namespace BoardGame.DataAccess.Models;

public class BoardGame
{
	public string Id { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public int ReleaseYear { get; set; }
	public string Description { get; set; } = string.Empty;
	public bool Available { get; set; }
	public List<Genre>? Genres { get; set; } = new ();
	public string? BggLink { get; set; } = string.Empty;
}