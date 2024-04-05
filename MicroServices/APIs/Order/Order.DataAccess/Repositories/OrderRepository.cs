using LendStuff.Shared;
using Microsoft.EntityFrameworkCore;

namespace Order.DataAccess.Repositories;

public class OrderRepository(OrderDbContext context) : IOrderRepository
{
	async Task<IEnumerable<Models.Order>> IRepository<Models.Order>.GetAll()
    {
        return await context.Orders.ToArrayAsync();
    }

    public async Task<Models.Order?> GetById(Guid id)
    {
        return await context.Orders.FirstOrDefaultAsync(o => o.OrderId == id);
    }

    public async Task<Models.Order> AddItem(Models.Order item)
	{
		context.Orders.Add(item);

		await context.SaveChangesAsync();

		return item;
	}

	public async Task<string> Delete(Guid id)
	{
		var toDelete = await context.Orders.FirstOrDefaultAsync(o => o.OrderId == id);

		if (toDelete is not null)
		{
			var result = context.Orders.Remove(toDelete);
			await context.SaveChangesAsync();
			return $"Order: {result.Entity.OrderId} removed";
		}

		return $"Order {id} not found";
	}
	
	public async Task<Models.Order?> Update(Models.Order item)
	{
		var toUpdate = await context.Orders.FirstOrDefaultAsync(o => o.OrderId.Equals(item.OrderId));

        if (toUpdate is null) return null;

        var properties = typeof(Models.Order).GetProperties();

        foreach (var propertyInfo in properties)
        {
            if (!propertyInfo.GetValue(item)!.Equals(propertyInfo.GetValue(toUpdate)))
            {
                propertyInfo.SetValue(toUpdate, propertyInfo.GetValue(item));
            }

            await context.SaveChangesAsync();
        }

        return toUpdate;
    }

    public async Task<IEnumerable<Models.Order>> GetByUserId(Guid userId)
    {
        return await context.Orders.Where(o => o.BorrowerId == userId || o.OwnerId == userId).ToArrayAsync();
    }
}