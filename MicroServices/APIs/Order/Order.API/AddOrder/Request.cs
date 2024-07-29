using FastEndpoints;
using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Order.API.Helpers;
using Order.DataAccess.Repositories;

namespace Order.API.AddOrder;

public record Request(Guid OwnerUserId, Guid BorrowerUserId, Guid BoardGameId);

public record Response(string Message, bool Success, OrderDto? OrderDto);

public class Handler(IOrderRepository repository, ICallClientHttpFactory callClientFactory)  : Endpoint<Request, Response>
{
	public override void Configure()
	{
		Post("/");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policies("Test");
    }

	public override async Task HandleAsync(Request req, CancellationToken ct)
	{
        var result = await callClientFactory.Call(req.BoardGameId);

        if (!result!.Available) await SendAsync(new Response("Boardgame not available", false, null), cancellation: ct);

        var newOrder = new DataAccess.Models.Order
        {
            OwnerId = result.OwnerId,
            BoardGameId = result.Id,
            BorrowerId = req.BorrowerUserId,
            LentDate = DateTime.Now,
            ReturnDate = DateTime.Now.AddDays(21),
            Status = OrderStatus.Inquiry
        };

        var response = await repository.AddItem(newOrder);

        await SendAsync(new Response($"Order with nr: {response.OrderId} added", true, OrderDtoConverter.ConvertOrderToDto(response)), cancellation: ct);
    }
}