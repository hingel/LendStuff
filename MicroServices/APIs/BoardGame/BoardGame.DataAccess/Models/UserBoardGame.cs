namespace BoardGame.DataAccess.Models;

public class UserBoardGame //Skulle kunna ärva av Boardgame. Så att den får dess klasser.
{
	public Guid UserBoardGameId { get; set; }
	public BoardGame BoardGame { get; set; }
	public int Condition { get; set; }
	public string? Comment { get; set; } = string.Empty;
	public bool ForLending { get; set; }
}