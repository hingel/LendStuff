using LendStuff.Shared;

namespace Messages.DataAccess.Models;

public class InternalMessage : IEntity
{
	public Guid Id { get; init; } = Guid.NewGuid();
	public string? Message { get; set; }
	public DateTime MessageSent { get; set; } = DateTime.UtcNow;
	public Guid SentToUserId { get; set; }
	public Guid SentFromUserId { get; set; }
	public bool IsRead { get; set; }

}