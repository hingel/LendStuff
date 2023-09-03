using Duende.IdentityServer.Stores;
using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using MediatR;

namespace LendStuff.Server.Queries;

public record GetAllGamesQuery() : IRequest<ServiceResponse<IEnumerable<BoardGameDto>>>;

