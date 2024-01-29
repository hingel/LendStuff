using LendStuff.DataAccess.Models;
using LendStuff.DataAccess.Repositories.Interfaces;
using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using Microsoft.AspNetCore.Identity;

namespace LendStuff.Server.Services;

public class UserService
{
	private readonly IRepository<ApplicationUser> _userRepository;
	private readonly UserManager<ApplicationUser> _userManager;

	public UserService(IRepository<ApplicationUser> userRepository, UserManager<ApplicationUser> userManager)
	{
		_userRepository = userRepository;
		_userManager = userManager;
	}

	public async Task<ServiceResponse<IEnumerable<string>>> GetUsersGames(string id)
	{ 
		Func<ApplicationUser, bool> filterFunc = (u) => u.Id == id;
		
		var result = await _userRepository.FindByKey(filterFunc);

		if (result.FirstOrDefault() is null)
		{
			return new ServiceResponse<IEnumerable<string>>()
			{
				Message = "Not found",
				Success = false
			};
		}

		return new ServiceResponse<IEnumerable<string>>()
		{
			Data = result.FirstOrDefault().CollectionOfBoardGameIds, //.Select(DtoConvert.ConvertUserBoardGameToDto),
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
					Rating = result.Rating,
					Email = result.Email,
					MessageDtos = result.Messages.Select(ConvertMessageToDto),
					//TODO: Fyll på ytterligare info här:
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
	//public async Task<ServiceResponse<string>> UpdateBoardGameUserCollection(BoardGameDto boardGameDto, string userId)
	//{
	//	var userToUpdate = (await _userRepository.FindByKey(u => u.Id == userId)).FirstOrDefault();
	//	//var boardGame = (await _boardGamerepo.FindByKey(b => b.Id == boardGameDto.Id)).FirstOrDefault();
		
	//	if (userToUpdate is null || boardGame is null)
	//	{
	//		return new ServiceResponse<string>()
	//		{
	//			Message = "User not found",
	//			Success = false
	//		};
	//	}

	//	if (userToUpdate.CollectionOfBoardGames.All(ub => ub.BoardGame.Id != boardGameDto.Id))
	//	{
	//		userToUpdate.CollectionOfBoardGames.Add(new UserBoardGame()
	//		{
	//			BoardGame = boardGame,
	//			ForLending = true
	//		});
	//	}
	//	else
	//	{
	//		var ub = userToUpdate.CollectionOfBoardGames.FirstOrDefault(ub => ub.BoardGame.Id == boardGame.Id);
	//		userToUpdate.CollectionOfBoardGames.Remove(ub);
	//	}
		
	//	var result = await _userRepository.Update(userToUpdate);

	//	return new ServiceResponse<string>()
	//	{
	//		Message = "Board games added hopefully",
	//		Success = true
	//	};
	//}

	//public async Task<ServiceResponse<IEnumerable<UserDto>>> GetUsersOwningCertainBoardGame(string boardGameId)
	//{
	//	Func<ApplicationUser, bool> filterFunc = (u) => u.CollectionOfBoardGames.Any(b => b.BoardGame.Id == boardGameId);

	//	var result = await _userRepository.FindByKey(filterFunc);

	//	return new ServiceResponse<IEnumerable<UserDto>>()
	//	{
	//		Data = result.Select(ConvertAppUserToDto),
	//		Message = "Users boardgames",
	//		Success = true
	//	};
	//}

	private UserDto ConvertAppUserToDto(ApplicationUser user)
	{
		return new UserDto()
		{
			Rating = user.Rating,
			UserName = user.UserName,
			Email = user.Email
		};
	}

    public async Task<ServiceResponse<UserDto>> FindUserByName(string userName)
    {
        Func<ApplicationUser, bool> filterFunc = (u) => u.UserName == userName;

        var result = (await _userRepository.FindByKey(filterFunc)).FirstOrDefault();

        if (result is not null)
        {
            return new ServiceResponse<UserDto>()
            {
                Data = new UserDto()
                {
                    UserName = result.UserName,
					Rating = result.Rating,
					Email = result.Email,
					MessageDtos = result.Messages.Select(ConvertMessageToDto),
					//TODO: Fyll på ytterligare info här:
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

    private MessageDto ConvertMessageToDto(InternalMessage messageToConvert)
    {
        return new MessageDto()
        {
			Message = messageToConvert.Message,
			IsRead = messageToConvert.IsRead,
			MessageSent = messageToConvert.MessageSent,
			SentFromUserName = messageToConvert.SentFromUserName,
			SentToUserName = messageToConvert.SentToUser.UserName
        };
    }
}