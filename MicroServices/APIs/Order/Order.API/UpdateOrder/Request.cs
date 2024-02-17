﻿using FastEndpoints;
using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using Order.API.Helpers;

namespace Order.API.UpdateOrder;

public record Request(OrderDto updatedOrderDto);

public record Response(string Message, bool Success, OrderDto updatedOrder);

public class Handler(IRepository<DataAccess.Models.Order> repository) : Endpoint<Request, Response>
{
	public override void Configure()
	{
		Patch("/");
		AllowAnonymous();
	}

	public override async Task HandleAsync(Request req, CancellationToken ct)
	{
		var updatedOrder = await OrderDtoConverter.ConvertDtoToOrder(req.updatedOrderDto);

		var result = await repository.Update(updatedOrder);
		
		await SendAsync(new Response("Order updated", true, OrderDtoConverter.ConvertOrderToDto(result)), 200, ct);
	}
}