using LendStuff.DataAccess.Services;
using LendStuff.Shared.DTOs;

namespace LendStuff.Server.Extensions;

public static class WebApplicationExtensions
{
	#region Förtesting

	public static WebApplication MapBoardGameEndPoints(this WebApplication app)
	{
		app.MapGet("/allGames", async (BoardGameService brepo) => await brepo.GetAll());
		app.MapPost("/addGame", async (BoardGameService brepo, BoardGameDto toAdd) => await brepo.AddTitle(toAdd));
		app.MapPatch("/updateGame", async (BoardGameService brepo, BoardGameDto newToUpdate) => await brepo.UpdateBoardGame(newToUpdate));
		app.MapDelete("/deleteGame", async (BoardGameService brepo, string idToDelete) => await brepo.DeleteBoardGame(idToDelete));
		app.MapGet("/getGameByTitle", async (BoardGameService service, string title) => await service.FindByTitle(title));
		app.MapGet("/getGameById", async (BoardGameService service, string id) => await service.FindById(id));
		return app;
	}

	public static WebApplication MapUserEndPoints(this WebApplication app)
	{
		app.MapGet("/getUsersGames", async (UserService service, string id) => await service.GetUsersGames(id));
		app.MapGet("/getById", async (UserService service, string id) => await service.FindUserById(id));
		app.MapPatch("/addBoardGameToUser", async (UserService service, BoardGameDto boardGameToAdd, string userEmail) => await service.AddBoardGameToUserCollection(boardGameToAdd, userEmail));
		return app;
	}

	public static WebApplication MapOrderEndPoints(this WebApplication app)
	{
		app.MapGet("/getOrders", async (OrderService service) => await service.GetAllOrders());
		app.MapGet("/allUserOrders", async (OrderService service, string userDtoId) => await service.GetAllUserOrders(userDtoId));
		app.MapPost("/postOrder", async (OrderService service, OrderDto newOrderDto) => await service.AddOrder(newOrderDto));
		app.MapPatch("/updateOrder", async (OrderService service, OrderDto orderToUpdate) => await service.UpdateOrder(orderToUpdate));
		app.MapDelete("/deleteOrder", async (OrderService service, string orderToDelete) => await service.DeleteOrder(orderToDelete));
		return app;
	}

	public static WebApplication MapMessageEndPoints(this WebApplication app)
	{
		//TODO: lägg till getAll. Men vet inte om den kommer att användas.
		app.MapPost("/addMessage", async (MessageService service, MessageDto newMessage) => await service.AddMessage(newMessage));
		 app.MapGet("/getUsersMessages", async (MessageService service, string name) => await service.GetUserMessages(name));
		app.MapDelete("/deleteMessage", async (MessageService service, int id) => await service.DeleteMessage(id));
		app.MapPatch("/updateMessage", async (MessageService service, MessageDto messageToUpdate) => await service.UpdateMessage(messageToUpdate));
		return app;
	}

	#endregion
}