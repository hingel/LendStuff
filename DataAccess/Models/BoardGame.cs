using System.ComponentModel.DataAnnotations;
using LendStuff.DataAccess.Models;

namespace LendStuff.Server.Models;

public class BoardGame
{
	[Required]
	public string Id { get; set; } = string.Empty;
	[Required]
	public string Title { get; set; } = string.Empty;
	[Required]
	public int ReleaseYear { get; set; }
	[Required]
	public string Description { get; set; } = string.Empty;
	public bool Available { get; set; }
	public List<Genre>? Genres { get; set; } = new ();

	public string? BggLink { get; set; } = string.Empty;
}