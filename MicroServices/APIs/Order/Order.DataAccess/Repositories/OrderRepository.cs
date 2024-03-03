using LendStuff.Shared;
using Microsoft.EntityFrameworkCore;

namespace Order.DataAccess.Repositories;

public class OrderRepository : IRepository<Models.Order>
{
	private readonly OrderDbContext _context;

	public OrderRepository(OrderDbContext context)
	{
		_context = context;
	}

	
	public async Task<IEnumerable<Models.Order>> FindByKey(Func<Models.Order, bool> findFunc)
	{
		return await _context
			.Orders.Where(findFunc).AsQueryable().ToListAsync();
	}


	async Task<IEnumerable<Models.Order>> IRepository<Models.Order>.GetAll()
	{
		throw new NotImplementedException();
	}

	public async Task<Models.Order> AddItem(Models.Order item)
	{
		_context.Orders.Add(item);

		await _context.SaveChangesAsync();

		return item;

	}

	public async Task<string> Delete(Guid id)
	{
		var todelete = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == id);

		if (todelete is not null)
		{
			var result = _context.Orders.Remove(todelete);
			await _context.SaveChangesAsync();
			return $"Order: {result.Entity.OrderId} removed";
		}

		return $"Order {id} not found";
	}
	
	public async Task<Models.Order> Update(Models.Order item)
	{
		var toUpdate = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId.Equals(item.OrderId));

		if (toUpdate is not null)
		{
			var propetyList = typeof(Models.Order).GetProperties();

			foreach (var propertyInfo in propetyList)
			{
				//TODO: Kan behövas null check
				if (!propertyInfo.GetValue(item).Equals(propertyInfo.GetValue(toUpdate)))
				{
					propertyInfo.SetValue(toUpdate, propertyInfo.GetValue(item));
				}

				var result = await _context.SaveChangesAsync();
			}

			return toUpdate;
		}

		return null;
	}
}