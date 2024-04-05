using LendStuff.Shared;

namespace Order.DataAccess.Repositories;

public interface IOrderRepository : IRepository<Models.Order>
{
    Task<IEnumerable<Models.Order>> GetByUserId(Guid userId);
}