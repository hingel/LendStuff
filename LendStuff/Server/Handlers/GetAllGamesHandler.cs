using LendStuff.DataAccess.Repositories.Interfaces;
using LendStuff.DataAccess.Services;
using LendStuff.Server.Models;
using LendStuff.Server.Queries;
using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using MediatR;

namespace LendStuff.Server.Handlers;

public class GetAllGamesHandler : IRequestHandler<GetAllGamesQuery, ServiceResponse<IEnumerable<BoardGameDto>>>
{
	private readonly IRepository<BoardGame> _boardGameRepository;

	public GetAllGamesHandler(IRepository<BoardGame> boardgameRepository)
	{
		_boardGameRepository = boardgameRepository; //TODO: Detta borde då gå direkt mot databasen tror jag när det är CQRS.
	}
	public async Task<ServiceResponse<IEnumerable<BoardGameDto>>> Handle(GetAllGamesQuery request, CancellationToken cancellationToken)
	{
		var result = await _boardGameRepository.GetAll();

		//TODO: fixa detta lite bättre:
		return new ServiceResponse<IEnumerable<BoardGameDto>>()
		{
			Data = result.Select(DtoConvert.ConvertBoardGameToDto),
			Message = "Boardgames found",
			Success = true
		};
	}
}