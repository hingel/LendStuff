using BoardGame.DataAccess.Models;
using LendStuff.Shared.DTOs;

namespace BoardGame.API.Helpers;

public static class DtoConvert
{
	public static BoardGameDto ConvertBoardGameToDto(BoardGame.DataAccess.Models.BoardGame boardGame)
	{
		return new BoardGameDto()
		{
			Available = boardGame.Available,
			BggLink = boardGame.BggLink,
			Description = boardGame.Description,
			Genres = boardGame.Genres.Select(g => g.Name).ToList(),
			Id = boardGame.Id,
			ReleaseYear = boardGame.ReleaseYear,
			Title = boardGame.Title
		};
	}

	public static UserBoardGameDto ConvertUserBoardGameToDto(UserBoardGame userBoardGame)
	{
		return new UserBoardGameDto()
		{
			BoardGameDto = ConvertBoardGameToDto(userBoardGame.BoardGame),
			ForLending = userBoardGame.ForLending,
		};
	}
}