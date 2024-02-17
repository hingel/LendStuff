using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using MediatR;

namespace BoardGame.API.CommandsAndQueries;

public record UpdateGameCommand(BoardGameDto GameToUpdate) : IRequest<ServiceResponse<BoardGameDto>>;