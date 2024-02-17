using FastEndpoints;
using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using Order.API.Helpers;

namespace Order.API.AddOrder;

public record Request(OrderDto OrderDto);

public record Response(string Message, bool Success, OrderDto? OrderDto);

public class Handler(IRepository<DataAccess.Models.Order> repository) : Endpoint<Request, Response>
{
	public override void Configure()
	{
		Post("/");
		AllowAnonymous();
	}

	public override async Task HandleAsync(Request req, CancellationToken ct)
	{
		var newOrder = await OrderDtoConverter.ConvertDtoToOrder(req.OrderDto);

		//TODO: Måste ske API-anrop till den andra servern. Kolla hur det görs.

		//if (!newOrder.BoardGame.Available)
		//{
		//	await SendAsync(new Response("fix this", false, OrderDtoConverter.ConvertOrderToDto(newOrder)), 200 , ct);
		//}

		var result = await repository.AddItem(newOrder);

		await SendAsync(new Response($"Order with nr: {result.OrderId} added", true, null), cancellation: ct);
	}
}