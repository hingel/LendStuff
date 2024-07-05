using LendStuff.Shared;
using MassTransit;
using Order.DataAccess.Repositories;

namespace Order.API.Consumers;

public class DeleteOrders(IOrderRepository orderRepository) : IConsumer<LendStuff.Shared.Messages.DeleteOrders>
{
    public async Task Consume(ConsumeContext<LendStuff.Shared.Messages.DeleteOrders> context)
    {
        var orders = await orderRepository.GetByUserId(context.Message.UserId);

        var ordersToDelete = orders.Where(o => o.Status != OrderStatus.Active);

        await orderRepository.Delete(ordersToDelete.Select(o => o.OrderId).ToArray());
        
        //TODO: skulle kunna publicera ett meddelande om att de är borttagna
    }
}