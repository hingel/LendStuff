namespace LendStuff.Shared.DTOs;

public class BoardGameDto
{
	public string Id { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public int ReleaseYear { get; set; }
	public string Description { get; set; } = string.Empty;
	public int Condition { get; set; }
	public string? Comment { get; set; } = string.Empty;
	public bool Available { get; set; }
	public List<string> Genres { get; set; } = new(); //Testa att ha detta som en lisa med strängar enbart.
	public string BggLink { get; set; } = string.Empty;
	//public ICollection<ApplicationUserDto> Users { get; set; } //Tror inte denna behövs i front end. Kan leta upp användarna som har spelet i backend.
}