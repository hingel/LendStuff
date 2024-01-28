using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using MediatR;

namespace Messages.API.CommandsAndQueries;

public record AddMessageCommand(MessageDto NewMessageDto) : IRequest<ServiceResponse<string>>;