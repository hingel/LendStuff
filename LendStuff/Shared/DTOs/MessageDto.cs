using System.ComponentModel.DataAnnotations;

namespace LendStuff.Shared.DTOs;

public class MessageDto
{	
	public int MessageId { get; set; }
	[Required]
	[StringLength(400, ErrorMessage = "To long message. Max 400 characters.")]
	public string Message { get; set; } = string.Empty;
	public DateTime MessageSent { get; set; } = DateTime.UtcNow;
	public string SentFromUserName { get; set; } = string.Empty;
	[Required]
	public string SentToUserName { get; set; } = string.Empty;
	public bool IsRead { get; set; } = false;
}