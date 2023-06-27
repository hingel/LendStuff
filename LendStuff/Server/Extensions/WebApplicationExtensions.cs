using LendStuff.DataAccess.Services;
using LendStuff.Shared.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LendStuff.Server.Extensions;

public static class WebApplicationExtensions
{

	public static WebApplication MapBoardGameEndPoints(this WebApplication app)
	{
		app.MapGet("/allGames", GetAllGamesHandler); //.AllowAnonymous(); //p => p.RequireUserName("c@cr.se")); //TODO: lägg till detta senare.
		app.MapPost("/addGame", AddGameHandler).RequireAuthorization();
		app.MapPatch("/updateGame", UpdateGameHandler).RequireAuthorization();
		app.MapDelete("/deleteGame", async (BoardGameService brepo, string idToDelete) => await brepo.DeleteBoardGame(idToDelete)).RequireAuthorization("admin_access");
		app.MapGet("/getGameByTitle", async (BoardGameService service, string title) => await service.FindByTitle(title));
		app.MapGet("/getGameById", async (BoardGameService service, string id) => await service.FindById(id));
		return app;
	}

	public static WebApplication MapUserEndPoints(this WebApplication app)
	{
		app.MapGet("/getUsersGames", async (UserService service, string id) => await service.GetUsersGames(id)); //.RequireAuthorization();
		app.MapGet("/getById", async (UserService service, string id) => await service.FindUserById(id)); //.RequireAuthorization();
		//app.MapPatch("/addBoardGameToUser", async (UserService service, UserDto userToDtoUpdate) => await service.AddBoardGameToUserCollection(userToDtoUpdate));
		app.MapPatch("/updateBoardGameToUser", async ([FromServices] UserService service, [FromBody] BoardGameDto boardGameToAdd, [FromQuery(Name = "id")] string userId) =>
		{
			return await service.UpdateBoardGameUserCollection(boardGameToAdd, userId);
		});
		app.MapGet("/usersOwningABoardGame", async (UserService service, [FromQuery(Name = "Id")] string boardGameId) => await service.GetUsersOwningCertainBoardGame(boardGameId)).RequireAuthorization();
		return app;
	}

	public static WebApplication MapOrderEndPoints(this WebApplication app)
	{
		app.MapGet("/getOrders", async (OrderService service) => await service.GetAllOrders()).RequireAuthorization("admin_access");
		app.MapGet("/allUserOrders", async (OrderService service, string userDtoId) => await service.GetAllUserOrders(userDtoId)).RequireAuthorization();
		app.MapPost("/postOrder", async (OrderService service, OrderDto newOrderDto) => await service.AddOrder(newOrderDto)).RequireAuthorization();
		app.MapPatch("/updateOrder", async (OrderService service, OrderDto orderToUpdate) => await service.UpdateOrder(orderToUpdate)).RequireAuthorization();
		app.MapDelete("/deleteOrder", async (OrderService service, string orderToDelete) => await service.DeleteOrder(orderToDelete)).RequireAuthorization("admin_access");
		return app;
	}

	public static WebApplication MapMessageEndPoints(this WebApplication app)
	{
		//TODO: lägg till getAll. Men vet inte om den kommer att användas.
		app.MapPost("/addMessage", async (MessageService service, MessageDto newMessage) => await service.AddMessage(newMessage)); //.RequireAuthorization();
		 app.MapGet("/getUsersMessages", async (MessageService service, string name) => await service.GetUserMessages(name)); //.RequireAuthorization();
		app.MapDelete("/deleteMessage", async (MessageService service, int id) => await service.DeleteMessage(id)); //.RequireAuthorization();
		app.MapPatch("/updateMessage", async (MessageService service, MessageDto messageToUpdate) => await service.UpdateMessage(messageToUpdate)); //.RequireAuthorization();
		return app;
	}

	private static async Task<IResult> GetAllGamesHandler(BoardGameService brepo)
	{
		var response = await brepo.GetAll();
		return response.Success ? Results.Ok(response) : Results.BadRequest(response);
	}

	private static async Task<IResult> AddGameHandler(BoardGameService brepo, BoardGameDto toAdd)
	{
		var response = await brepo.AddTitle(toAdd);

		return response.Success ? Results.Ok(response) : Results.BadRequest(response);
	}

	private static async Task<IResult> UpdateGameHandler(BoardGameService brepo, BoardGameDto newToUpdate)
	{
		var response = await brepo.UpdateBoardGame(newToUpdate);

		return response.Success ? Results.Ok(response) : Results.BadRequest(response);
	}


}