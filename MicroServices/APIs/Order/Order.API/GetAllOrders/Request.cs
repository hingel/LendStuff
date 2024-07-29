using FastEndpoints;
using LendStuff.Shared.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Order.API.Helpers;
using Order.DataAccess.Repositories;

namespace Order.API.GetAllOrders;

public record Request();

public record Response(string Message, bool Success, OrderDto[] OrderDtos);

public class Handler(IOrderRepository repository) : Endpoint<Request, Response>
{
	public override void Configure()
	{
		Get("/");
        //AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        AllowAnonymous();
    }

	public override async Task HandleAsync(Request req, CancellationToken ct)
	{
		var result = await repository.GetAll();
		
		await SendAsync(new Response("All orders", true, result.Select(OrderDtoConverter.ConvertOrderToDto).ToArray()), 200 , ct);
	}
}