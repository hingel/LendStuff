namespace LendStuff.Shared.DTOs;

public class BoardGameDto
{
	public string Id { get; set; }
	public string Title { get; set; }
	public int? ReleaseYear { get; set; }
	public string Description { get; set; }
	public int Condition { get; set; }
	public string? Comment { get; set; }
	public bool Available { get; set; }
	public List<string>? Genres { get; set; } = new(); //Testa att ha detta som en lisa med strängar enbart.

	public string BggLink { get; set; }
	//public ICollection<ApplicationUserDto> Users { get; set; } //Tror inte denna behövs i front end. Kan leta upp användarna som har spelet i backend.
}