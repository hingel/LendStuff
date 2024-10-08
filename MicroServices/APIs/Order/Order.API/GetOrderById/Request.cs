﻿using FastEndpoints;
using LendStuff.Shared.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Order.API.Helpers;
using Order.DataAccess.Repositories;

namespace Order.API.GetOrderById;

public record Request(Guid OrderId);

public record Response(string Message, bool Success, OrderDto? OrderDto);

public class Handler(IOrderRepository repository) : Endpoint<Request, Response>
{
	public override void Configure()
	{
		Get("/{orderId}");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policies("Test");
	}

	public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var result = await repository.GetById(req.OrderId);

		if (result is null)
		{
			await SendAsync(new Response($"Order with id: {req.OrderId} not found", true, null), 404, ct);
			return;
		}
		
		await SendAsync(new Response($"Order with id: { req.OrderId }", true, OrderDtoConverter.ConvertOrderToDto(result)), 200, ct);
	}
}