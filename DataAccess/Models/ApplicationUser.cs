using LendStuff.DataAccess.Models;
using Microsoft.AspNetCore.Identity;

namespace LendStuff.Server.Models;

public class ApplicationUser : IdentityUser
{
	public int Rating { get; set; } //TODO: Kan ha en metod för att räkna ut snitt istället?
	public ICollection<BoardGame> CollectionOfBoardGames { get; set; } = new List<BoardGame>();
	public ICollection<InternalMessage> Messages { get; set; } = new List<InternalMessage>();
	public DateTime RegisterDate { get; set; } = DateTime.UtcNow;

}