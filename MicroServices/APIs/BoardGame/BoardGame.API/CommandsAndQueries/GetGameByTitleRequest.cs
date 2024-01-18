using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using MediatR;

namespace BoardGame.API.CommandsAndQueries;

public record GetGameByTitleRequest(string Title) : IRequest<ServiceResponse<IEnumerable<BoardGameDto>>>;