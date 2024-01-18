using LendStuff.DataAccess.Repositories;
using LendStuff.DataAccess.Services;
using LendStuff.Server.Queries;
using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using MediatR;

namespace LendStuff.Server.Handlers;

public class GetGameByIdHandler : IRequestHandler<GetGameByIdQuery, ServiceResponse<BoardGameDto>>
{
	private readonly BoardGameRepository _repository;

	public GetGameByIdHandler(BoardGameRepository repository)
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