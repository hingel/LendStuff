using LendStuff.DataAccess.Repositories.Interfaces;
using LendStuff.Server.Models;
using LendStuff.Shared;
using Microsoft.AspNetCore.Identity;

namespace LendStuff.DataAccess.Services;

public class UserService
{
	private readonly IRepository<BoardGame> _boardGameRepository;
	private readonly IRepository<ApplicationUser> _userRepository;
	private readonly UserManager<ApplicationUser> _userManager;

	public UserService(IRepository<BoardGame> boardGameRepository, IRepository<ApplicationUser> userRepository, UserManager<ApplicationUser> userManager)
	{
		_boardGameRepository = boardGameRepository;
		_userRepository = userRepository;
		_userManager = userManager;
	}

	public async Task<ServiceResponse<IEnumerable<BoardGame>>> GetUsersGames(string email)
	{ 
		Func<ApplicationUser, bool> filterFunc = (u) => u.NormalizedEmail == email.ToUpper();
		
		var result = await _userRepository.FindByKey(filterFunc);
		
		return new ServiceResponse<IEnumerable<BoardGame>>()
		{
			Data = result.FirstOrDefault().CollectionOfBoardGames,
			Message = "BoardGames found",
			Success = true
		};
	}


	//Denna metod borde kansek inte finnas? Eller iaf kommer det en DTO från frontend.
	public async Task<ServiceResponse<ApplicationUser>> AddBoardGameToUserCollection(BoardGame toAdd, string email)
	{
		var userGames = await GetUsersGames(email);

		var games = userGames.Data.ToList();

		games.Add(toAdd);

		var userToUpdate = await _userManager.FindByEmailAsync(email);

		//Här borde jag spara databasen.

		userToUpdate.CollectionOfBoardGames = games;

		var result = await _userRepository.Update(userToUpdate);

		return new ServiceResponse<ApplicationUser>()
		{
			Data = result,
			Message = "Board games added hopefully",
			Success = true
		};
	}
}