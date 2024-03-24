using FastEndpoints;
using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Order.API.Helpers;

namespace Order.API.GetAllOrders;

public record Request;

public record Response(string Message, bool Success, OrderDto[] OrderDtos);

public class Handler(IRepository<DataAccess.Models.Order> repository) : Endpoint<Request, Response>
{
	public override void Configure()
	{
		Get("/");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
    }

	public override async Task HandleAsync(Request req, CancellationToken ct)
	{
		var result = await repository.GetAll();
		
		await SendAsync(new Response("All orders", true, result.Select(OrderDtoConverter.ConvertOrderToDto).ToArray()), 200 , ct);
	}
}