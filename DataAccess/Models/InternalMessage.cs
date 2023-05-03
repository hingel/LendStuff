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
	public string SentFromUserGuid { get; set; }
	public string SentToUserGuid { get; set;}

}