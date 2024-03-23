using LendStuff.Shared;

namespace BoardGame.DataAccess.Models;

public class BoardGame : IEntity
{
	public Guid Id { get; init; } = Guid.NewGuid();
	public string Title { get; set; } = string.Empty;
	public int ReleaseYear { get; set; }
	public string Description { get; set; } = string.Empty;
	public bool Available { get; set; }
	public ICollection<Genre> Genres { get; set; } = new List<Genre>();
	public string? BggLink { get; set; } = string.Empty;
    public Guid OwnerId { get; set; }
}