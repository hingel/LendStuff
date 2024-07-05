using BoardGame.API.CommandsAndQueries;
using BoardGame.API.Helpers;
using BoardGame.DataAccess.Repository;
using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using MediatR;

namespace BoardGame.API.Handlers;

public class GetGameByIdHandler(IBoardGameRepository repository)
    : IRequestHandler<GetGameByIdQuery, ServiceResponse<BoardGameDto>>
{
    public async Task<ServiceResponse<BoardGameDto>> Handle(GetGameByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetById(request.Id);

		if (result != null)
		{
			return new ServiceResponse<BoardGameDto>()
			{
				Data = DtoConvert.ConvertBoardGameToDto(result),
				Message = $"Boardgame found.",
				Success = true
			};
		}

		return new ServiceResponse<BoardGameDto>()
		{
			Message = "No board game found",
			Success = false
		};
	}
}