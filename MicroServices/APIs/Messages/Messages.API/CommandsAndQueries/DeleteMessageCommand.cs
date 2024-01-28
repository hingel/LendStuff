using LendStuff.Shared;
using MediatR;

namespace Messages.API.CommandsAndQueries;

public record DeleteMessageCommand(Guid MessageId) : IRequest<ServiceResponse<string>>;