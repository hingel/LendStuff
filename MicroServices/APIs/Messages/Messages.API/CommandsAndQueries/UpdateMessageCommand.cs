using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using MediatR;

namespace Messages.API.CommandsAndQueries;

public record UpdateMessageCommand(MessageDto MessageToUpdateDto) : IRequest<ServiceResponse<MessageDto>>;