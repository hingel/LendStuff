using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using MediatR;

namespace BoardGame.API.CommandsAndQueries;

public record GetGameByIdQuery(Guid Id) : IRequest<ServiceResponse<BoardGameDto>>;