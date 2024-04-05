using FastEndpoints;
using LendStuff.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Order.API.DeleteOrder;

public record Request(Guid OrderId);

public record Response(string Message, bool Success);

public class Handler(IRepository<DataAccess.Models.Order> repository) : Endpoint<Request, Response>
{
	public override void Configure()
	{
		Delete("/{orderId}");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
	}

	public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var currentOrder = await repository.GetById(req.OrderId);

		if( currentOrder is null)
        {
            await SendAsync(new Response("Deleted", true), 200, ct);
            return;
        }

        if (currentOrder.Status == OrderStatus.Active)
        {
            await SendAsync(new Response("Can not delete", false), 404 ,cancellation: ct);
        }

        var result = await repository.Delete(req.OrderId);
		await SendAsync(new Response(result, true), 200, ct);
	}
}