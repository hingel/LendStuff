using BoardGame.API.CommandsAndQueries;
using BoardGame.API.Helpers;
using BoardGame.DataAccess.Repository;
using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using MediatR;

namespace BoardGame.API.Handlers;

public class GetGameByIdHandler : IRequestHandler<GetGameByIdQuery, ServiceResponse<BoardGameDto>>
{
	private readonly IRepository<DataAccess.Models.BoardGame> _repository;

	public GetGameByIdHandler(IRepository<DataAccess.Models.BoardGame> repository)
	{
		_repository = repository;
	}
	public async Task<ServiceResponse<BoardGameDto>> Handle(GetGameByIdQuery request, CancellationToken cancellationToken)
	{
		var result = await _repository.FindByKey((game => game.Id == request.Id));

		if (result.Any())
		{
			return new ServiceResponse<BoardGameDto>()
			{
				Data = result.Select(DtoConvert.ConvertBoardGameToDto).First(),
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