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

	public async Task<string> Delete(params Guid[] ids)
	{
		var toDeletes = await context.Orders.Where(o => ids.Contains(o.OrderId)).ToArrayAsync();

        if (!toDeletes.Any()) return $"Order {string.Join(", ", ids)} not found";

		context.Orders.RemoveRange(toDeletes);
		await context.SaveChangesAsync();
		return $"Order: {string.Join(", ", toDeletes.Select(td => td.OrderId))} removed";
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