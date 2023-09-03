using LendStuff.DataAccess.Models;
using LendStuff.DataAccess.Repositories;
using LendStuff.DataAccess.Repositories.Interfaces;
using LendStuff.DataAccess.Services;
using LendStuff.Server.Commands;
using LendStuff.Server.Models;
using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using MediatR;

namespace LendStuff.Server.Handlers;

public class AddGameHandler : IRequestHandler<AddGameCommand, ServiceResponse<BoardGameDto>>
{
	private readonly UnitOfWork _unitOfWork;
	private readonly IRepository<BoardGame> _boardGameRepository;

	public AddGameHandler(UnitOfWork unitOfWork, IRepository<BoardGame> boardGameRepository)
	{
		_unitOfWork = unitOfWork;
		_boardGameRepository = boardGameRepository;
	}

	public async Task<ServiceResponse<BoardGameDto>> Handle(AddGameCommand request, CancellationToken cancellationToken)
	{
		if (!(await _boardGameRepository.FindByKey(b => b.Title.ToLower() == request.BoardGameToAdd.Title.ToLower())).Any())
		{
			var response = await _unitOfWork.BoardGameRepository.AddItem(await ConvertDtoToBoardGame(request.BoardGameToAdd));

			if (response is not null)
			{
				await _unitOfWork.SaveChanges();

				return new ServiceResponse<BoardGameDto>()
				{
					Data = DtoConvert.ConvertBoardGameToDto(response),
					Message = "BoardGame added",
					Success = true
				};
			}
		}

		return new ServiceResponse<BoardGameDto>() { Message = "BoardGame not added", Success = false };
	}

	private async Task<BoardGame> ConvertDtoToBoardGame(BoardGameDto dtoToConvert)
	{
		return new BoardGame
		{
			Available = dtoToConvert.Available,
			BggLink = dtoToConvert.BggLink,
			Comment = dtoToConvert.Comment,
			Condition = dtoToConvert.Condition,
			Description = dtoToConvert.Description,
			Genres = await FindGenres(dtoToConvert.Genres),
			Id = dtoToConvert.Id == "" ? Guid.NewGuid().ToString() : dtoToConvert.Id,
			ReleaseYear = dtoToConvert.ReleaseYear,
			Title = dtoToConvert.Title
		};
	}

	private async Task<List<Genre>> FindGenres(List<string> genreStrings)
	{
		var listOfGenres = new List<Genre>();

		foreach (var genre in genreStrings)
		{
			var search =
				await _unitOfWork.GenreRepository.FindByKey(g => g.Name.ToLower().Equals(genre.ToLower())); //TODO: mer check på detta. Kapa tomutrymme etc.
			//Eller ge förslag på när användaren skriver i de ord som är liknande.

			if (search.Count() > 0)
			{
				listOfGenres.AddRange(search);
				continue;
			}

			//Här skapas en ny genre om den inte redan finns.
			listOfGenres.Add(await _unitOfWork.GenreRepository.AddItem(new Genre() { Name = genre }));
			await _unitOfWork.SaveChanges();
		}

		return listOfGenres;

	}
}