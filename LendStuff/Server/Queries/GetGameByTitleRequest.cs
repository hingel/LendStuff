using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using MediatR;

namespace LendStuff.Server.Queries;

public record GetGameByTitleRequest(string Title) : IRequest<ServiceResponse<IEnumerable<BoardGameDto>>>;