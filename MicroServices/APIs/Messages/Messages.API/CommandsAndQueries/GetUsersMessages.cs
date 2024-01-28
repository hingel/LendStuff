using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using MediatR;

namespace Messages.API.CommandsAndQueries;

public record GetUsersMessages(Guid UserId) : IRequest<ServiceResponse<IEnumerable<MessageDto>>>;