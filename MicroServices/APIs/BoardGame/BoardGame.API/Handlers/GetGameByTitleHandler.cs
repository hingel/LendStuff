using BoardGame.API.CommandsAndQueries;
using BoardGame.API.Helpers;
using BoardGame.DataAccess.Repository;
using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using MediatR;

namespace BoardGame.API.Handlers;

public class GetGameByTitleHandler : IRequestHandler<GetGameByTitleRequest, ServiceResponse<IEnumerable<BoardGameDto>>>
{
	private readonly BoardGameRepository _repository;

	public GetGameByTitleHandler(BoardGameRepository repository)
	{
		_repository = repository;
	}
	public async Task<ServiceResponse<IEnumerable<BoardGameDto>>> Handle(GetGameByTitleRequest request, CancellationToken cancellationToken)
	{
		var result = await _repository.FindByKey((game => game.Title.ToLower().Contains(request.Title.ToLower())));

		return new ServiceResponse<IEnumerable<BoardGameDto>>
		{
			Data = result.Select(DtoConvert.ConvertBoardGameToDto),
			Message = $"{result.Count()} Boardgame{(result.Count() != 1 ? "s" : "")} found.",
			Success = true
		};
	}
}