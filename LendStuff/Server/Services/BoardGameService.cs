using LendStuff.DataAccess.Models;
using LendStuff.DataAccess.Repositories;
using LendStuff.DataAccess.Repositories.Interfaces;
using LendStuff.Server.Models;
using LendStuff.Shared;
using LendStuff.Shared.DTOs;

namespace LendStuff.DataAccess.Services;

public class BoardGameService
{
	private readonly UnitOfWork _unitOfWork;

	public BoardGameService(UnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<ServiceResponse<IEnumerable<BoardGameDto>>> GetAll()
	{
		var result = await _unitOfWork.BoardGameRepository.GetAll();

		//TODO: fixa detta lite bättre:
		return new ServiceResponse<IEnumerable<BoardGameDto>>()
		{
			Data = result.Select(DtoConvert.ConvertBoardGameToDto),
			Message = "Boardgames found",
			Success = true
		};
	}

	//TODO: Borde kunna skicka detta delegatet ifrån frontend?!
	public async Task<ServiceResponse<IEnumerable<BoardGameDto>>> FindByTitle(string searchWord)
	{
		var result = await _unitOfWork.BoardGameRepository.FindByKey((game => game.Title.ToLower().Contains(searchWord.ToLower())));
		
		//TODO: fixa detta lite bättre:
		return new ServiceResponse<IEnumerable<BoardGameDto>>()
		{
			Data = result.Select(DtoConvert.ConvertBoardGameToDto),
			Message = $"{result.Count()} Boardgame{(result.Count() != 1 ? "s": "")} found.",
			Success = true
		};
	}

	public async Task<ServiceResponse<BoardGameDto>> FindById(string id)
	{
		var result = await _unitOfWork.BoardGameRepository.FindByKey((game => game.Id == id));

		if (result.Count() > 0)
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


	public async Task<ServiceResponse<BoardGameDto>> AddTitle(BoardGameDto toAdd)
	{
		//Kolla först om det redan finns en liknande titel med samma namn? 
		var result = await _unitOfWork.BoardGameRepository.FindByKey((game => game.Title.ToLower().Contains(toAdd.Title.ToLower())));

		if (result.Count() > 0)
		{
			return new ServiceResponse<BoardGameDto>()
			{
				Data = DtoConvert.ConvertBoardGameToDto(result.First()),
				Message = "Game already exists",
				Success = false //TODO: Vet inte om det är rätt med detta eftersom jag inte får tillbaks resultatet till front end
			};
		}

		//annars lägg till en ny titel.
		var addResult = await _unitOfWork.BoardGameRepository.AddItem(await ConvertDtoToBoardGame(toAdd));
		var saveResult = await _unitOfWork.SaveChanges();

		return new ServiceResponse<BoardGameDto>()
		{
			Data = DtoConvert.ConvertBoardGameToDto(addResult),
			Message = "Board game added",
			Success = true
		};
	}

	public async Task<ServiceResponse<string>> DeleteBoardGame(string id)
	{
		var result = await _unitOfWork.BoardGameRepository.Delete(id);
		var saveResult = await _unitOfWork.SaveChanges();

		return new ServiceResponse<string>()
		{
			Message = result,
			Success = true
		};
	}

	public async Task<ServiceResponse<BoardGameDto>> UpdateBoardGame(BoardGameDto gameDto)
	{
		var result = await _unitOfWork.BoardGameRepository.Update(await ConvertDtoToBoardGame(gameDto));
		
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
		return new BoardGame()
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
		}

		return listOfGenres;
		
	}

}