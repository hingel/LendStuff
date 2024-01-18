using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using MediatR;

namespace LendStuff.Server.Queries;

public record GetGameByIdQuery(string Id) : IRequest<ServiceResponse<BoardGameDto>>;