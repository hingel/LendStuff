using System.ComponentModel.DataAnnotations;
using LendStuff.Server.Models;

namespace LendStuff.DataAccess.Models;

public class UserBoardGame
{
	[Key]
	public Guid UserBoardGameId { get; set; } = Guid.NewGuid();
	public BoardGame BoardGame { get; set; }
	public bool ForLending { get; set; }
}