using LendStuff.Shared;

namespace BoardGame.DataAccess.Repository;

public interface IBoardGameRepository : IRepository<Models.BoardGame>
{
    Task<IEnumerable<Models.BoardGame>> GetByUserId(Guid userId);
    Task<IEnumerable<Models.BoardGame>> GetByTitle(string searchWord);
}