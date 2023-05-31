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
	private readonly IRepository<BoardGame> _boardGamerepo;

	public UserService(IRepository<ApplicationUser> userRepository, UserManager<ApplicationUser> userManager, IRepository<BoardGame> boardGameRepo)
	{
		_userRepository = userRepository;
		_userManager = userManager;
		_boardGamerepo = boardGameRepo;
	}

	public async Task<ServiceResponse<IEnumerable<BoardGameDto>>> GetUsersGames(string id)
	{ 
		Func<ApplicationUser, bool> filterFunc = (u) => u.Id == id;
		
		var result = await _userRepository.FindByKey(filterFunc); //dessa spel borde göras om till DTO.

		if (result.FirstOrDefault() is null)
		{
			return new ServiceResponse<IEnumerable<BoardGameDto>>()
			{
				Message = "Not found",
				Success = false
			};
		}

		return new ServiceResponse<IEnumerable<BoardGameDto>>()
		{
			Data = result.FirstOrDefault().CollectionOfBoardGames.Select(DtoConvert.ConvertBoardGameToDto),
			Message = "BoardGames found",
			Success = true
		};
	}


	//Behövs denna? För admin kanske?
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
	//TODO Detta funkat inte som tänkt i nuläget. 
	public async Task<ServiceResponse<string>> UpdateBoardGameUserCollection(BoardGameDto boardGameDto, string userId)
	{
		var userToUpdate = (await _userRepository.FindByKey(u => u.Id == userId)).FirstOrDefault();
		var boardGame = (await _boardGamerepo.FindByKey(b => b.Id == boardGameDto.Id)).FirstOrDefault();
		
		if (userToUpdate is null || boardGame is null)
		{
			return new ServiceResponse<string>()
			{
				Message = "User not found",
				Success = false
			};
		}

		if (userToUpdate.CollectionOfBoardGames.All(b => b.Id != boardGameDto.Id))
		{
			userToUpdate.CollectionOfBoardGames.Add(boardGame);
		}
		else
		{
			userToUpdate.CollectionOfBoardGames.Remove(boardGame);
		}
		
		var result = await _userRepository.Update(userToUpdate);

		return new ServiceResponse<string>()
		{
			Message = "Board games added hopefully",
			Success = true
		};
	}
}