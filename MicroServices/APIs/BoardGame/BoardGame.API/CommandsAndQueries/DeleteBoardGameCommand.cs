using LendStuff.Shared;
using MediatR;

namespace BoardGame.API.CommandsAndQueries;

public record DeleteBoardGameCommand(string BoardGameId) : IRequest<ServiceResponse<string>>;