using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using MediatR;

namespace LendStuff.Server.Commands;

public record UpdateGameCommand(BoardGameDto gameToUpdate) : IRequest<ServiceResponse<BoardGameDto>>;