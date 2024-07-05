using BoardGame.API.CommandsAndQueries;
using BoardGame.API.Helpers;
using BoardGame.DataAccess.Models;
using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using MediatR;

namespace BoardGame.API.Handlers;

public class UpdateGameHandler(UnitOfWork unitOfWork)
    : IRequestHandler<UpdateGameCommand, ServiceResponse<BoardGameDto>>
{
    public async Task<ServiceResponse<BoardGameDto>> Handle(UpdateGameCommand request, CancellationToken cancellationToken)
	{
		var result = await unitOfWork.BoardGameRepository.Update(await ConvertDtoToBoardGame(request.GameToUpdate));

		if (result is null)
		{
			return new ServiceResponse<BoardGameDto>()
			{
				Message = "Boardgame not updated.",
				Success = false
			};
		}

		var saveResult = await unitOfWork.SaveChanges();

		return new ServiceResponse<BoardGameDto>()
		{
			Data = DtoConvert.ConvertBoardGameToDto(result),
			Message = "Boardgame updated.",
			Success = true
		};
	}

    public async Task<DataAccess.Models.BoardGame> ConvertDtoToBoardGame(BoardGameDto dtoToConvert)
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

    public async Task<List<Genre>> FindGenres(List<string> genreStrings)
    {
        var listOfGenres = new List<Genre>();

        foreach (var genre in genreStrings)
        {
            var search = await unitOfWork.GenreRepository.GetByName(genre.Trim());  //TODO: Kanske borde hämta alla o kontrollera dem?

            if (search.Any())
            {
                listOfGenres.AddRange(search);
                continue;
            }

            listOfGenres.Add(await unitOfWork.GenreRepository.AddItem(new Genre { Name = genre.Trim().ToLowerInvariant() }));
            await unitOfWork.SaveChanges();
        }

        return listOfGenres;
    }
}