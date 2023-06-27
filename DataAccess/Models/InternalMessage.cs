using System.ComponentModel.DataAnnotations;
using LendStuff.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace LendStuff.DataAccess.Models;

public class InternalMessage
{
	[Key]
	public int MessageId { get; set; }
	[Required, MaxLength(400)]
	public string Message { get; set; }
	public DateTime MessageSent { get; set; } = DateTime.UtcNow;
	[Required]
	public ApplicationUser SentToUser { get; set; }
	[Required]
	public string SentFromUserName { get; set; }
	public bool IsRead { get; set; }
}