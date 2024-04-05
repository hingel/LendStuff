using BoardGame.DataAccess.Models;
using LendStuff.Shared;
using Microsoft.EntityFrameworkCore;

namespace BoardGame.DataAccess.Repository;

public class GenreRepository : IGenreRepository
{
	private readonly BoardGameDbContext _context;
	public GenreRepository(BoardGameDbContext context)
	{
		_context = context;
	}

	public async Task<IEnumerable<Genre>> GetAll()
	{
		return await _context.Genres.ToArrayAsync();
	}

    public async Task<Genre?> GetById(Guid id)
    {
        return await _context.Genres.FirstOrDefaultAsync(g => g.Id == id);
    }
	
	public async Task<Genre> AddItem(Genre item)
	{
		var result = await _context.Genres.AddAsync(item);

		return result.Entity;
	}

	public async Task<string> Delete(Guid id)
	{
		var toDelete = await _context.Genres.FirstOrDefaultAsync(g => g.Id.Equals(id));

		if (toDelete != null)
		{
			var result = _context.Genres.Remove(toDelete);
			
			return $"Genre: {result.Entity.Name} deleted";
		}

		return "Genre not found.";
	}

	public async Task<Genre?> Update(Genre item)
	{
		var toUpdate = await _context.Genres.FirstOrDefaultAsync(g => g.Id == item.Id);

		if (toUpdate != null)
		{
			var propertyList = typeof(Genre).GetProperties();

			foreach (var prop in propertyList)
			{
				if (!prop.GetValue(item).Equals(prop.GetValue(toUpdate)))
				{
					prop.SetValue(toUpdate, prop.GetValue(item));
				}
			}
		}

		return toUpdate;
	}

    public async Task<IEnumerable<Genre>> GetByName(string name)
    {
        return await _context.Genres.Where(g => g.Name.ToLower() == name.ToLower()).ToArrayAsync();
    }
}