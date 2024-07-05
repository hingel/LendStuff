namespace LendStuff.Shared.Messages;

public record ChangeBoardGameAvailability(Guid BoardGameId, bool Availability);