using BoardGame.API.CommandsAndQueries;
using LendStuff.Shared.DTOs;
using MediatR;

namespace BoardGame.API.Extensions;

public static class WebApplicationExtensions
{
	public static WebApplication MapBoardGameEndPoints(this WebApplication app)
	{
		app.MapGet("/allGames", GetAllGamesHandler); //.RequireCors("myCorsSpec"); //.AllowAnonymous(); //p => p.RequireUserName("c@cr.se")); //TODO: lägg till detta senare.
		app.MapPost("/addGame", AddGameHandler); //.RequireAuthorization();
		app.MapPatch("/updateGame", async (IMediator mediator, BoardGameDto boardGameToUpdate) =>
		{
			var response = await mediator.Send(new UpdateGameCommand(boardGameToUpdate));
			return response.Success ? Results.Ok(response) : Results.BadRequest(response);
		}).RequireAuthorization();
		app.MapDelete("/deleteGame", async (IMediator mediator, Guid idToDelete) =>
		{
			var response = await mediator.Send(new DeleteBoardGameCommand(idToDelete));
			return response.Success ? Results.Ok(response) : Results.BadRequest(response);
		}); //.RequireAuthorization("admin_access");
		app.MapGet("/getGameByTitle", async (IMediator mediator, string title) =>
		{
			var response = await mediator.Send(new GetGameByTitleRequest(title));
			return response.Success ? Results.Ok(response) : Results.BadRequest(response);

		});
		app.MapGet("/getGameById", async (IMediator mediator, Guid id) =>
		{
			var response = await mediator.Send(new GetGameByIdQuery(id));
			return response.Success ? Results.Ok(response) : Results.BadRequest(response);
		});
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