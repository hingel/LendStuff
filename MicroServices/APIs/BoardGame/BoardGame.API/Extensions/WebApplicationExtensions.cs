﻿using BoardGame.API.CommandsAndQueries;
using LendStuff.Shared.DTOs;
using MediatR;

namespace BoardGame.API.Extensions;

public static class WebApplicationExtensions
{
	public static WebApplication MapBoardGameEndPoints(this WebApplication app)
    {
        app.MapGet("/allGames", GetAllGamesHandler); //.RequireCors("myCorsSpec"); //.AllowAnonymous();
		app.MapPost("/addGame", AddGameHandler).RequireAuthorization();
		app.MapPatch("/updateGame", async (IMediator mediator, BoardGameDto boardGameToUpdate) =>
		{
			var response = await mediator.Send(new UpdateGameCommand(boardGameToUpdate));
			return response.Success ? Results.Ok(response) : Results.BadRequest(response);
		}).RequireAuthorization();
		app.MapDelete("/deleteGame/{idToDelete}", async (IMediator mediator, string idToDelete) =>
		{
			var response = await mediator.Send(new DeleteBoardGameCommand(Guid.Parse(idToDelete)));
			return response.Success ? Results.Ok(response) : Results.BadRequest(response);
		}).RequireAuthorization();
		app.MapGet("/getGameByTitle/{title}", async (IMediator mediator, string title) =>
		{
			var response = await mediator.Send(new GetGameByTitleRequest(title));
			return response.Success ? Results.Ok(response) : Results.BadRequest(response);
		}).RequireAuthorization();
		app.MapGet("/getGameById/{id}", async (IMediator mediator, string id) =>
		{
			var response = await mediator.Send(new GetGameByIdQuery(Guid.Parse(id)));
			return response.Success ? Results.Ok(response) : Results.BadRequest(response);
		}).RequireAuthorization();
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