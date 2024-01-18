using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using MediatR;

namespace BoardGame.API.CommandsAndQueries;

public record GetAllGamesQuery() : IRequest<ServiceResponse<IEnumerable<BoardGameDto>>>;