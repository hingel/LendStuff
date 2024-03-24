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
		var result = await repository.Delete(req.OrderId);


		await SendAsync(new Response(result, true), 200, ct);
	}
}