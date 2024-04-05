using BoardGame.API.CommandsAndQueries;
using BoardGame.API.Helpers;
using BoardGame.DataAccess.Repository;
using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using MediatR;

namespace BoardGame.API.Handlers;

public class GetGameByTitleHandler(IBoardGameRepository repository)
    : IRequestHandler<GetGameByTitleRequest, ServiceResponse<IEnumerable<BoardGameDto>>>
{
    public async Task<ServiceResponse<IEnumerable<BoardGameDto>>> Handle(GetGameByTitleRequest request, CancellationToken cancellationToken)
	{
        var result = await repository.GetByTitle(request.Title);

		return new ServiceResponse<IEnumerable<BoardGameDto>>
		{
			Data = result.Select(DtoConvert.ConvertBoardGameToDto),
			Message = $"{result.Count()} BoardGame{(result.Count() != 1 ? "s" : "")} found.",
			Success = true
		};
	}
}