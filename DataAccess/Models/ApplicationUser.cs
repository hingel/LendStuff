using LendStuff.DataAccess.Models;
using Microsoft.AspNetCore.Identity;

namespace LendStuff.Server.Models;

public class ApplicationUser : IdentityUser
{
	public int Rating { get; set; } //TODO: Kan ha en metod för att räkna ut snitt istället?
	public virtual ICollection<UserBoardGame> CollectionOfBoardGames { get; set; } = new List<UserBoardGame>(); //detta ska istället vara en lista med UserGames. Som 
	public virtual ICollection<InternalMessage> Messages { get; set; } = new List<InternalMessage>();
	public DateTime RegisterDate { get; set; } = DateTime.UtcNow;

	//TODO: Ska det även vara med kundens ordar om jag vill länka det åt bägge håll?

}