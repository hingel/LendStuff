namespace LendStuff.Shared.DTOs;

public class MessageDto
{	
	public int MessageId { get; set; }
	public string Message { get; set; } = string.Empty;
	public DateTime MessageSent { get; set; } = DateTime.UtcNow;
	public string SentFromUserGuid { get; set; } = string.Empty;
	public string SentToUserGuid { get; set; } = string.Empty;
}