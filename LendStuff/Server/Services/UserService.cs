using LendStuff.DataAccess.Repositories.Interfaces;
using LendStuff.Server.Models;
using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using Microsoft.AspNetCore.Identity;
using static Duende.IdentityServer.Models.IdentityResources;

namespace LendStuff.DataAccess.Services;

public class UserService
{
	private readonly IRepository<ApplicationUser> _userRepository;
	private readonly UserManager<ApplicationUser> _userManager;

	public UserService(IRepository<ApplicationUser> userRepository, UserManager<ApplicationUser> userManager)
	{
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

	public async Task<ServiceResponse<UserDto>> FindUserById(string id)
	{
		Func<ApplicationUser, bool> filterFunc = (u) => u.Id == id;

		var result = (await _userRepository.FindByKey(filterFunc)).FirstOrDefault();

		if (result is not null)
		{
			return new ServiceResponse<UserDto>()
			{
				Data = new UserDto()
				{
					UserName = result.UserName,
				},
				Message = "User found",
				Success = true

			};
		}

		return new ServiceResponse<UserDto>()
		{
			Message = "user not found",
			Success = false
		};
	}


	//Denna metod borde kanske inte finnas? Eller iaf kommer det en DTO från frontend.
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