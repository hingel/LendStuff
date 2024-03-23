using LendStuff.Server.Services;
using LendStuff.Shared.DTOs;

namespace LendStuff.Server.Extensions;

public static class WebApplicationExtensions
{
	public static WebApplication MapUserEndPoints(this WebApplication app)
	{
		app.MapGet("/getUsersGames/{id}", async (UserService service, string id) => await service.GetUsersGames(id)); //.RequireAuthorization();
		app.MapGet("/getUserById/{id}", async (UserService service, string id) => await service.FindUserById(id)); //.RequireAuthorization();
		//app.MapPatch("/addBoardGameToUser", async (UserService service, UserDto userToDtoUpdate) => await service.AddBoardGameToUserCollection(userToDtoUpdate));
		//app.MapPatch("/updateBoardGameToUser", async ([FromServices] UserService service, [FromBody] BoardGameDto boardGameToAdd, [FromQuery(Name = "id")] string userId) =>
		//{
		//	return await service.UpdateBoardGameUserCollection(boardGameToAdd, userId);
		//});
		//app.MapGet("/usersOwningBoardGame", async (UserService service, [FromQuery(Name = "boardGameId")] string boardGameId) => await service.GetUsersOwningCertainBoardGame(boardGameId)).RequireAuthorization();
        app.MapGet("/getUserByUserName/{userName}", async (UserService service, string userName) => await service.FindUserByName(userName)); //.RequireAuthorization();
		return app;
    }
}