using FastEndpoints;
using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using Order.API.Helpers;

namespace Order.API.GetAnUserOrders;

public record Request(Guid UserId);

public record Response(string Message, bool Success, OrderDto[]? OrderDtos);

public class Handler(IRepository<DataAccess.Models.Order> repository) : Endpoint<Request, Response>
{
	public override void Configure()
	{
		Get("/{userId}");
		AllowAnonymous();
	}

	public override async Task HandleAsync(Request req, CancellationToken ct)
	{
		var result = await repository.FindByKey(o => o.OwnerId == req.UserId || o.BorrowerId == req.UserId);

		if (result.FirstOrDefault() is null)
		{
			await SendAsync(new Response("All User orders", false, null), 404, ct);
			return;
		}

		await SendAsync(new Response($"order for {req.UserId}", true, result.Select(OrderDtoConverter.ConvertOrderToDto).ToArray()), 200, ct);
	}
}