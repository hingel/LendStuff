using LendStuff.DataAccess.Models;
using LendStuff.DataAccess.Repositories;
using LendStuff.DataAccess.Repositories.Interfaces;
using LendStuff.Server.Models;
using LendStuff.Shared;
using LendStuff.Shared.DTOs;

namespace LendStuff.DataAccess.Services;

public class BoardGameService
{
	private readonly IRepository<BoardGame> _boardGameRepository;

	public BoardGameService(IRepository<BoardGame> boardGameRepository)
	{
		_boardGameRepository = boardGameRepository;
	}

	public async Task<ServiceResponse<IEnumerable<BoardGameDto>>> GetAll()
	{
		var result = await _boardGameRepository.GetAll();

		//TODO: fixa detta lite bättre:
		return new ServiceResponse<IEnumerable<BoardGameDto>>()
		{
			Data = result.Select(ConvertBoardGameToDto),
			Message = "Boardgames found",
			Success = true
		};
	}

	//TODO: Borde kunna skicka detta delegatet ifrån frontend?!
	public async Task<ServiceResponse<IEnumerable<BoardGameDto>>> FindByTitle(string searchWord)
	{
		var result = await _boardGameRepository.FindByKey((game => game.Title.Contains(searchWord)));
		
		//TODO: fixa detta lite bättre:
		return new ServiceResponse<IEnumerable<BoardGameDto>>()
		{
			Data = result.Select(ConvertBoardGameToDto),
			Message = $"{result.Count()} Boardgame {(result.Count() != 1 ? "": "s.")} found.",
			Success = true
		};
	}

	public async Task<ServiceResponse<BoardGameDto>> AddTitle(BoardGameDto toAdd)
	{
		//Kolla först om det redan finns en liknande titel med samma namn? 
		var result = await _boardGameRepository.FindByKey((game => game.Title.ToLower().Contains(toAdd.Title.ToLower())));

		if (result is not null)
		{
			return new ServiceResponse<BoardGameDto>()
			{
				Data = ConvertBoardGameToDto(result.First()),
				Message = "Game already exists",
				Success = false //TODO: Vet inte om det är rätt med detta eftersom jag inte får tillbaks resultatet
			};
		}

		//annars lägg till en ny titel.
		var addResult = await _boardGameRepository.AddItem(ConvertDtoToBoardGame(toAdd));

		return new ServiceResponse<BoardGameDto>()
		{
			Data = ConvertBoardGameToDto(addResult),
			Message = "Board game added",
			Success = true
		};
	}

	public async Task<ServiceResponse<string>> DeleteBoardGame(string id)
	{
		var result = await _boardGameRepository.Delete(id);

		return new ServiceResponse<string>()
		{
			Message = result,
			Success = true
		};
	}

	public async Task<ServiceResponse<BoardGameDto>> UpdateBoardGame(BoardGameDto gameDto)
	{
		var result = await _boardGameRepository.Update(ConvertDtoToBoardGame(gameDto));

		if (result is null)
		{
			return new ServiceResponse<BoardGameDto>()
			{
				Message = "Boardgame not updated",
				Success = false
			};
		}

		return new ServiceResponse<BoardGameDto>()
		{
			Data = ConvertBoardGameToDto(result),
			Message = "Boardgame updated",
			Success = true
		};
	}

	private BoardGameDto ConvertBoardGameToDto(BoardGame boardGame)
	{
		return new BoardGameDto()
		{
			Available = boardGame.Available,
			BggLink = boardGame.BggLink,
			Comment = boardGame.Comment,
			Condition = boardGame.Condition,
			Description = boardGame.Description,
			Genres = boardGame.Genres.Select(g => g.Name).ToList(),
			Id = boardGame.Id,
			ReleaseYear = boardGame.ReleaseYear,
			Title = boardGame.Title
		};
	}

	private BoardGame ConvertDtoToBoardGame(BoardGameDto dtoToConvert)
	{
		return new BoardGame()
		{
			Available = dtoToConvert.Available,
			BggLink = dtoToConvert.BggLink,
			Comment = dtoToConvert.Comment,
			Description = dtoToConvert.Description,
			Genres = FindGenres(dtoToConvert.Genres),
		};
	}

	private List<Genre> FindGenres(List<string> GenreStrings)
	{
		//1.Kolla om genren redan existeras, isf lägg till den existerande
		//2. Annars skapa en ny.


	}

}