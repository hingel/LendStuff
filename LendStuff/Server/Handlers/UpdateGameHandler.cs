using LendStuff.DataAccess.Models;
using LendStuff.DataAccess.Services;
using LendStuff.Server.Commands;
using LendStuff.Server.Models;
using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;

namespace LendStuff.Server.Handlers;

public class UpdateGameHandler : IRequestHandler<UpdateGameCommand, ServiceResponse<BoardGameDto>>
{
	private readonly UnitOfWork _unitOfWork;

	public UpdateGameHandler(UnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<ServiceResponse<BoardGameDto>> Handle(UpdateGameCommand request, CancellationToken cancellationToken)
	{
		var result = await _unitOfWork.BoardGameRepository.Update(await ConvertDtoToBoardGame(request.gameToUpdate));

		if (result is null)
		{
			return new ServiceResponse<BoardGameDto>()
			{
				Message = "Boardgame not updated.",
				Success = false
			};
		}

		var saveResult = await _unitOfWork.SaveChanges();

		return new ServiceResponse<BoardGameDto>()
		{
			Data = DtoConvert.ConvertBoardGameToDto(result),
			Message = "Boardgame updated.",
			Success = true
		};
	}

	private async Task<BoardGame> ConvertDtoToBoardGame(BoardGameDto dtoToConvert)
	{
		return new BoardGame
		{
			Available = dtoToConvert.Available,
			BggLink = dtoToConvert.BggLink,
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
		}

		return listOfGenres;
	}
}