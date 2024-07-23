using System.Net.Http.Json;
using AutoFixture;
using LendStuff.Shared;
using Microsoft.Extensions.DependencyInjection;
using Order.API.GetOrderById;
using Order.DataAccess;

namespace IntegrationTests.Order;

public class OrderTests(IntegrationTestFactory<Program> factory) : IntegrationTestHelper(factory)
{
    
    [Fact]
    public async Task TestGetOrders_ReturnsOrders()
    {
        var order = Fixture.Create<global::Order.DataAccess.Models.Order>();

        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
        await dbContext.Database.EnsureCreatedAsync();

        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync();
        
        var response = await HttpClient.GetAsync($"/{order.OrderId}");
        var result = await response.Content.ReadFromJsonAsync<Response>();

        Assert.NotNull(result);

    }
}
        //using var readScope = Factory.Services.CreateScope();
        //var readContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
        //var test = readContext.Orders.FirstOrDefault();