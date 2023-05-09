using System.ComponentModel.DataAnnotations;
using LendStuff.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace LendStuff.DataAccess.Models;

public class InternalMessage
{
	[Key]
	public int MessageId { get; set; }
	public string Message { get; set; }
	public DateTime MessageSent { get; set; } = DateTime.UtcNow;
	public ApplicationUser SentToUser { get; set; }
	public string SentFromUserName { get; set; }
	//lägga till om det är läst eller ej, bool

}