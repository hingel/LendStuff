using Microsoft.AspNetCore.Identity;

namespace LendStuff.DataAccess.Models;

public class ApplicationUser : IdentityUser
{
	public int Rating { get; set; } //TODO: Kan ha en metod för att räkna ut snitt istället?
	public virtual ICollection<string> CollectionOfBoardGameIds { get; set; } = new List<string>();
	public virtual ICollection<Guid> MessagesIds { get; set; } = new List<Guid>();
	public DateTime RegisterDate { get; set; } = DateTime.UtcNow;

	//TODO: Ska det även vara med kundens ordar om jag vill länka det åt bägge håll?

}