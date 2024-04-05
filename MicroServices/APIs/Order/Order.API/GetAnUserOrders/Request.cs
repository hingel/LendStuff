using FastEndpoints;
using LendStuff.Shared.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Order.API.Helpers;
using Order.DataAccess.Repositories;

namespace Order.API.GetAnUserOrders;

public record Request(Guid UserId);

public record Response(string Message, bool Success, OrderDto[]? OrderDtos);

public class Handler(IOrderRepository repository) : Endpoint<Request, Response>
{
	public override void Configure()
	{
		Get("/getuserorders/{userId}");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
    }

	public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var result = await repository.GetByUserId(req.UserId);

		if (result.FirstOrDefault() is null)
		{
			await SendAsync(new Response("All User orders", false, null), 404, ct);
			return;
		}

		await SendAsync(new Response($"order for {req.UserId}", true, result.Select(OrderDtoConverter.ConvertOrderToDto).ToArray()), 200, ct);
	}
}