namespace LendStuff.Shared.DTOs;

public class MessageDto
{	
	public int MessageId { get; set; }
	public string Message { get; set; } = string.Empty;
	public DateTime MessageSent { get; set; } = DateTime.UtcNow;
	public string SentFromUserName { get; set; } = string.Empty;
	public string SentToUserName { get; set; } = string.Empty;
}