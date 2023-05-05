using LendStuff.DataAccess.Models;
using LendStuff.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LendStuff.DataAccess.Repositories;

public class OrderRepository : IRepository<Order>
{
	private readonly ApplicationDbContext _context;

	public OrderRepository(ApplicationDbContext context)
	{
		_context = context;
	}


	public async Task<IEnumerable<Order>> GetAll()
	{
		var respons = _context
			.Orders
			.Include(o => o.Owner)
			.Include(o => o.Borrower)
			.Include(o => o.BoardGame);

		return respons;
	}

	public async Task<IEnumerable<Order>> FindByKey(Func<Order, bool> findFunc)
	{
		return _context.Orders
			.Include(o => o.Owner)
			.Include(o => o.Borrower)
			.Include(o => o.BoardGame)
			.Where(findFunc);
	}

	public async Task<Order> AddItem(Order item)
	{
		await _context.Orders.AddAsync(item);

		await _context.SaveChangesAsync();

		return item;

	}

	public async Task<string> Delete(string id)
	{
		var todelete = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId.ToString() == id);

		if (todelete is not null)
		{
			var result = _context.Orders.Remove(todelete);
			return $"Order: {result.Entity.OrderId} removed";
		}

		return $"Order {id} not found";
	}

	public async Task<Order> Update(Order item)
	{
		var toUpdate = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId.Equals(item.OrderId));

		if (toUpdate is not null)
		{
			var propetyList = typeof(Order).GetProperties();

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