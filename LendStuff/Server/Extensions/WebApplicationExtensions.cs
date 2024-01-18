using LendStuff.DataAccess.Services;
using LendStuff.Server.Commands;
using LendStuff.Server.Queries;
using LendStuff.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LendStuff.Server.Extensions;

public static class WebApplicationExtensions
{
	public static WebApplication MapBoardGameEndPoints(this WebApplication app)
	{
		app.MapGet("/allGames", GetAllGamesHandler).RequireCors("myCorsSpec"); //.AllowAnonymous(); //p => p.RequireUserName("c@cr.se")); //TODO: lägg till detta senare.
		app.MapPost("/addGame", AddGameHandler).RequireAuthorization();
		app.MapPatch("/updateGame", async (IMediator mediator, BoardGameDto boardGameToUpdate) =>
		{
			var response = await mediator.Send(new UpdateGameCommand(boardGameToUpdate));
			return response.Success ? Results.Ok(response) : Results.BadRequest(response);
		}).RequireAuthorization();
		app.MapDelete("/deleteGame", async (IMediator mediator, string idToDelete) =>
		{
			var response = await mediator.Send(new DeleteBoardGameCommand(idToDelete));
			return response.Success ? Results.Ok(response) : Results.BadRequest(response);
		}).RequireAuthorization("admin_access");
		app.MapGet("/getGameByTitle", async (IMediator mediator, string title) =>
		{
			var response = await mediator.Send(new GetGameByTitleRequest(title));
			return response.Success ? Results.Ok(response) : Results.BadRequest(response);

		});
		app.MapGet("/getGameById", async (IMediator mediator, string id) =>
		{
			var response = await mediator.Send(new GetGameByIdQuery(id));
			return response.Success ? Results.Ok(response) : Results.BadRequest(response);
		});
		return app;
	}

	public static WebApplication MapUserEndPoints(this WebApplication app)
	{
		app.MapGet("/getUsersGames", async (UserService service, string id) => await service.GetUsersGames(id)).RequireAuthorization();
		app.MapGet("/getById", async (UserService service, string id) => await service.FindUserById(id)).RequireAuthorization();
		//app.MapPatch("/addBoardGameToUser", async (UserService service, UserDto userToDtoUpdate) => await service.AddBoardGameToUserCollection(userToDtoUpdate));
		app.MapPatch("/updateBoardGameToUser", async ([FromServices] UserService service, [FromBody] BoardGameDto boardGameToAdd, [FromQuery(Name = "id")] string userId) =>
		{
			return await service.UpdateBoardGameUserCollection(boardGameToAdd, userId);
		});
		app.MapGet("/usersOwningBoardGame", async (UserService service, [FromQuery(Name = "boardGameId")] string boardGameId) => await service.GetUsersOwningCertainBoardGame(boardGameId)).RequireAuthorization();
        app.MapGet("/getUserByUserName", async (UserService service, string userName) => await service.FindUserByName(userName)).RequireAuthorization();
		return app;
    }

	public static WebApplication MapOrderEndPoints(this WebApplication app)
	{
		app.MapGet("/getOrders", async (OrderService service) => await service.GetAllOrders()); //.RequireAuthorization("admin_access"); //TODO: Lägga till admin access
		app.MapGet("/allUserOrders", async (OrderService service, string userDtoId) => await service.GetAllUserOrders(userDtoId)).RequireAuthorization();
		app.MapPost("/postOrder", async (OrderService service, OrderDto newOrderDto) => await service.AddOrder(newOrderDto)).RequireAuthorization();
		app.MapPatch("/updateOrder", async (OrderService service, OrderDto orderToUpdate) => await service.UpdateOrder(orderToUpdate)).RequireAuthorization();
		app.MapDelete("/deleteOrder", async (OrderService service, string orderToDelete) => await service.DeleteOrder(orderToDelete)).RequireAuthorization("admin_access");
		app.MapGet("/getByOrderId", async (OrderService service, int orderId) => await service.GetOrderById(orderId)).RequireAuthorization();
		return app;
	}

	public static WebApplication MapMessageEndPoints(this WebApplication app)
	{
		//TODO: lägg till getAll. Men vet inte om den kommer att användas.
		app.MapPost("/addMessage", async (MessageService service, MessageDto newMessage) => await service.AddMessage(newMessage)).RequireAuthorization();
		 app.MapGet("/getUsersMessages", async (MessageService service, string name) => await service.GetUserMessages(name)).RequireAuthorization();
		app.MapDelete("/deleteMessage", async (MessageService service, int id) => await service.DeleteMessage(id)).RequireAuthorization();
		app.MapPatch("/updateMessage", async (MessageService service, MessageDto messageToUpdate) => await service.UpdateMessage(messageToUpdate)).RequireAuthorization();
		return app;
	}

	private static async Task<IResult> GetAllGamesHandler(IMediator mediator)
	{
		var response = await mediator.Send(new GetAllGamesQuery());
		return response.Success ? Results.Ok(response) : Results.BadRequest(response);
	}

	private static async Task<IResult> AddGameHandler(IMediator mediator, BoardGameDto toAdd)
	{
		var response = await mediator.Send(new AddGameCommand(toAdd));

		return response.Success ? Results.Ok(response) : Results.BadRequest(response);
	}
}