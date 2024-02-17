using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using MediatR;

namespace BoardGame.API.CommandsAndQueries;

public record AddGameCommand(BoardGameDto BoardGameToAdd) : IRequest<ServiceResponse<BoardGameDto>>;