using System.Net.Http.Json;
using System.Text;
using AutoFixture;
using FakeItEasy;
using LendStuff.Shared.DTOs;
using LendStuff.Shared.Messages;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Order.API.GetOrderById;
using Order.API.Helpers;
using Order.DataAccess;

namespace IntegrationTests.Order;

public class OrderTests(IntegrationTestFactory<Program> factory) : IntegrationTestHelper(factory)
{
    [Fact]
    public async Task TestGetOrderByIds_ReturnsOrders()
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
        Assert.NotNull(result.OrderDto);
        Assert.True(result.Success);
        Assert.Equal($"Order with id: {order.OrderId}", result.Message);
        Assert.Equivalent(order, new
        {
            result.OrderDto.OrderId,
            OwnerId = result.OrderDto.OwnerUserId,
            BorrowerId = result.OrderDto.BorrowerUserId,
            result.OrderDto.BoardGameId,
            result.OrderDto.LentDate,
            result.OrderDto.ReturnDate,
            result.OrderDto.Status,
            result.OrderDto.OrderMessageGuids
        });
    }

    [Fact]
    public async Task DeleteOrder_ConsumesMessage()
    {
        var order = Fixture.Create<global::Order.DataAccess.Models.Order>();

        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
        await dbContext.Database.EnsureCreatedAsync();

        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync();

        await TestHarness.Bus.Publish(new DeleteOrders(order.OwnerId));
        Assert.True(await TestHarness.Consumed.Any<DeleteOrders>(x => x.Context.Message.UserId == order.OwnerId));

        using var readScope = Factory.Services.CreateScope();
        var readContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
        await readContext.Database.EnsureCreatedAsync();

        Assert.Null(readContext.Orders.FirstOrDefault(o => o.OrderId == order.OrderId));
    }

    //TODO: detta testet failer, då det inte går att mocka bort ICLient factory i Fastendpoints endpointen, utan att implemeterna deras test paket.
    [Fact]
    public async Task Add_Order_ReturnsResponse()
    {
        var request = Fixture.Create<global::Order.API.AddOrder.Request>();

        var boardGame = Fixture.Build<BoardGameDto>()
            .With(b => b.Id, request.BoardGameId)
            .With(b => b.Available, true).Create();

        var clientFactory = A.Fake<ICallClientHttpFactory>();
      
        A.CallTo(() => clientFactory.Call(request.BoardGameId)).Returns(boardGame);

        var orderContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        await HttpClient.PostAsync("/", orderContent);

        
    }
}