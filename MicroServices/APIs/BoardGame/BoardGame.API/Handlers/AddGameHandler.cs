using BoardGame.API.CommandsAndQueries;
using BoardGame.API.Helpers;
using BoardGame.DataAccess.Models;
using BoardGame.DataAccess.Repository;
using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using MediatR;

namespace BoardGame.API.Handlers;

public class AddGameHandler : IRequestHandler<AddGameCommand, ServiceResponse<BoardGameDto>>
{
	private readonly UnitOfWork _unitOfWork;
	private readonly IRepository<BoardGame.DataAccess.Models.BoardGame> _boardGameRepository;

	public AddGameHandler(UnitOfWork unitOfWork, BoardGame.DataAccess.Repository.IRepository<BoardGame.DataAccess.Models.BoardGame> boardGameRepository)
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

	private async Task<DataAccess.Models.BoardGame> ConvertDtoToBoardGame(BoardGameDto dtoToConvert)
	{
		return new DataAccess.Models.BoardGame
		{
			Available = dtoToConvert.Available,
			BggLink = dtoToConvert.BggLink,
			Description = dtoToConvert.Description,
			Genres = await FindGenres(dtoToConvert.Genres),
			Id = Guid.NewGuid(),
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

			if (search.Any())
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