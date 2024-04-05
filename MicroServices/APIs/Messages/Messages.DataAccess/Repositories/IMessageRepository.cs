using LendStuff.Shared;
using Messages.DataAccess.Models;

namespace Messages.DataAccess.Repositories;

public interface IMessageRepository : IRepository<InternalMessage>
{
    Task<IEnumerable<InternalMessage>> GetByUserId(Guid userId);
}