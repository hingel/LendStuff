using System.ComponentModel.DataAnnotations;

namespace LendStuff.Shared.DTOs;

public class UserBoardGameDto //Denna klass skullle egentligen ärva av boardgameDto.
{
	public BoardGameDto BoardGameDto { get; set; }

	[Range(1, 5, ErrorMessage = "Range 1 - 5. 5 equals mint condition.")]
	public int Condition { get; set; }
	public string? Comment { get; set; } = string.Empty;
	[Required]
	public bool ForLending { get; set; }
}