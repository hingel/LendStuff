using BoardGame.DataAccess.Models;
using LendStuff.Shared;

namespace BoardGame.DataAccess.Repository;

public interface IGenreRepository : IRepository<Genre>
{
    Task<IEnumerable<Genre>> GetByName(string name);
}