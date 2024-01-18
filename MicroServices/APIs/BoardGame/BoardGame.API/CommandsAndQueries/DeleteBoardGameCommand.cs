using LendStuff.Shared;
using MediatR;

namespace BoardGame.API.CommandsAndQueries;

public record DeleteBoardGameCommand(Guid Id) : IRequest<ServiceResponse<string>>;