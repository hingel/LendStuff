using LendStuff.Shared;
using MediatR;

namespace LendStuff.Server.Commands;

public record DeleteBoardGameCommand(string BoardGameId) : IRequest<ServiceResponse<string>>;