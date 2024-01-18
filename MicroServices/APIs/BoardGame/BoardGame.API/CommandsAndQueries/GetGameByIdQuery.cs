using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using MediatR;

namespace BoardGame.API.CommandsAndQueries;

public record GetGameByIdQuery(string Id) : IRequest<ServiceResponse<BoardGameDto>>;