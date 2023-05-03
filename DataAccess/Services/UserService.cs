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

	public async Task<ServiceResponse<IEnumerable<BoardGame>>> AddBoardGameToUserCollection(BoardGame toAdd, string email)
	{
		var userToUpdate = await _userManager.FindByEmailAsync();
	}
}