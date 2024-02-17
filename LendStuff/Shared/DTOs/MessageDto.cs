using System.ComponentModel.DataAnnotations;

namespace LendStuff.Shared.DTOs;

public class MessageDto
{	
	public Guid MessageId { get; init; }
	[Required]
	[StringLength(400, ErrorMessage = "To long message. Max 400 characters.")]
	public string Message { get; set; } = string.Empty;
	public DateTime MessageSent { get; set; } = DateTime.UtcNow;
	[Required]
	public Guid SentFromUserId { get; set; }
	[Required]
	public Guid SentToUserId{ get; set; }
	public bool IsRead { get; set; } = false;
}