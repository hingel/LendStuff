﻿using FastEndpoints;
using LendStuff.Shared.DTOs;
using LendStuff.Shared;
using Order.API.Helpers;
using Order.DataAccess.Models;
using Order.DataAccess.Repositories;

namespace Order.API.GetOrderById;

public record Request(Guid OrderId);

public record Response(string Message, bool Success, OrderDto? OrderDtos);

public class Handler(IRepository<DataAccess.Models.Order> repository) : Endpoint<Request, Response>
{
	public override void Configure()
	{
		Get("/");
		AllowAnonymous();
	}

	public override async Task HandleAsync(Request req, CancellationToken ct)
	{
		var result = (await repository.FindByKey(o => o.OrderId == req.OrderId)).FirstOrDefault();

		if (result is null)
		{
			await SendAsync(new Response($"Order with id: {req.OrderId} not found", true, null), 404, ct);
			return;
		}
		
		await SendAsync(new Response("All orders", true, OrderDtoConverter.ConvertOrderToDto(result)), 200, ct);
	}
}