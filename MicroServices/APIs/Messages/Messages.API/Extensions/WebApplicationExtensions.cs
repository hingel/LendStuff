using LendStuff.Shared.DTOs;
using MediatR;
using Messages.API.CommandsAndQueries;

namespace Messages.API.Extensions;

public static class WebApplicationExtensions
{
	public static WebApplication MapMessageEndPoints(this WebApplication app)
	{
		app.MapPost("/addMessage", async (IMediator mediator, MessageDto newMessage) => await mediator.Send(new AddMessageCommand(newMessage))).RequireAuthorization();
		app.MapGet("/getUsersMessages", async (IMediator mediator, Guid UserId) => await mediator.Send(new GetUsersMessages(UserId))).RequireAuthorization();
		app.MapDelete("/deleteMessage", async (IMediator mediator, Guid MessageId) => await mediator.Send(new DeleteMessageCommand(MessageId))).RequireAuthorization();
		app.MapPatch("/updateMessage", async (IMediator mediator, MessageDto messageToUpdate) => await mediator.Send(new UpdateMessageCommand(messageToUpdate))).RequireAuthorization();
		return app;
	}
}