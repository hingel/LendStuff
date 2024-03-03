using LendStuff.Shared.DTOs;
using MediatR;
using Messages.API.CommandsAndQueries;
using Microsoft.AspNetCore.Mvc;

namespace Messages.API.Extensions;

public static class WebApplicationExtensions
{
	public static WebApplication MapMessageEndPoints(this WebApplication app)
	{
		app.MapPost("/addMessage", async (IMediator mediator, MessageDto newMessage) =>
			await mediator.Send(new AddMessageCommand(newMessage)));//.RequireAuthorization();
		app.MapGet("/getUsersMessages", async (IMediator mediator, string userId) => await mediator.Send(new GetUsersMessages(Guid.Parse(userId)))); //.RequireAuthorization(););
		app.MapDelete("/deleteMessage", async (IMediator mediator, string messageId) =>
			await mediator.Send(new DeleteMessageCommand(Guid.Parse(messageId)))); //.RequireAuthorization();
		app.MapPatch("/updateMessage", async (IMediator mediator, MessageDto messageToUpdate) =>
			await mediator.Send(new UpdateMessageCommand(messageToUpdate))); //.RequireAuthorization();
		return app;
	}
}