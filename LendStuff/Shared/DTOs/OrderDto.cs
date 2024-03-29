namespace LendStuff.Shared.DTOs;

public record OrderDto(
    Guid? OrderId,
    Guid OwnerUserId,
    Guid BorrowerUserId,
    Guid BoardGameId,
    DateTime? LentDate,
    DateTime? ReturnDate,
    OrderStatus? Status,
    Guid[] OrderMessageGuids);