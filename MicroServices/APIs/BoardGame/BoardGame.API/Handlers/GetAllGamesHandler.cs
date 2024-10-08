﻿using BoardGame.API.CommandsAndQueries;
using BoardGame.API.Helpers;
using BoardGame.DataAccess.Repository;
using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using MediatR;

namespace BoardGame.API.Handlers;

public class GetAllGamesHandler : IRequestHandler<GetAllGamesQuery, ServiceResponse<IEnumerable<BoardGameDto>>>
{
	private readonly IBoardGameRepository _boardGameRepository;

	public GetAllGamesHandler(IBoardGameRepository boardGameRepository)
	{
		_boardGameRepository = boardGameRepository; //TODO: Detta borde då gå direkt mot databasen tror jag när det är CQRS.
	}
	public async Task<ServiceResponse<IEnumerable<BoardGameDto>>> Handle(GetAllGamesQuery request, CancellationToken cancellationToken)
	{
		var result = await _boardGameRepository.GetAll();

		return new ServiceResponse<IEnumerable<BoardGameDto>>()
		{
			Data = result.Select(DtoConvert.ConvertBoardGameToDto),
			Message = "Boardgames found",
			Success = true
		};
	}
}