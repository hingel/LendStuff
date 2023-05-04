namespace LendStuff.Shared.DTOs;

public class MessageDto
{	
	public int MessageId { get; set; }
	public string Message { get; set; }
	public DateTime MessageSent { get; set; } = DateTime.UtcNow;
	public string SentFromUserGuid { get; set; }
	public string SentToUserGuid { get; set; }
}