using LendStuff.Shared.DTOs;
using MediatR;
using Messages.API.CommandsAndQueries;
using Microsoft.AspNetCore.Mvc;

namespace Messages.API.Extensions;

public static class WebApplicationExtensions
{
	public static WebApplication MapMessageEndPoints(this WebApplication app)
	{
		app.MapPost("/messages", async (IMediator mediator, [FromBody] MessageDto newMessage) =>
			await mediator.Send(new AddMessageCommand(newMessage)));//.RequireAuthorization();
		app.MapGet("/messages/{id}", async (IMediator mediator, string id) => await mediator.Send(new GetUsersMessages(Guid.Parse(id)))); //.RequireAuthorization(););
		app.MapDelete("/messages/{id}", async (IMediator mediator, string id) =>
			await mediator.Send(new DeleteMessageCommand(Guid.Parse(id)))); //.RequireAuthorization();
		app.MapPatch("/messages", async (IMediator mediator, [FromBody] MessageDto messageToUpdate) =>
			await mediator.Send(new UpdateMessageCommand(messageToUpdate))); //.RequireAuthorization();
		return app;
	}
}