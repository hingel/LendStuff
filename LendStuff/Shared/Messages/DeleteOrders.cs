using MassTransit;

namespace LendStuff.Shared.Messages;

public record DeleteOrders(Guid UserId);