using System.ComponentModel.DataAnnotations;

namespace LendStuff.Shared.DTOs;

public class BoardGameDto
{
	public string Id { get; set; } = string.Empty;
	[Required]
	public string Title { get; set; } = string.Empty;
	[Required]
	[Range(1970, 2050, ErrorMessage = "Latest 1970")]
	public int ReleaseYear { get; set; }
	[Required]
	public string Description { get; set; } = string.Empty;
	[Required]
	public bool Available { get; set; }
	public List<string> Genres { get; set; } = new(); //Testa att ha detta som en lisa med strängar enbart.
	public string? BggLink { get; set; } = string.Empty;
}