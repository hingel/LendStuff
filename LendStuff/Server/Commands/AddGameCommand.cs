using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using MediatR;

namespace LendStuff.Server.Commands;

public record AddGameCommand(BoardGameDto BoardGameToAdd) : IRequest<ServiceResponse<BoardGameDto>>;