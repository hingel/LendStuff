using BoardGame.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace BoardGame.DataAccess.Repository;

public class GenreRepository : IRepository<Genre>
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

	public async Task<IEnumerable<Genre>> FindByKey(Func<Genre, bool> findFunc)
	{
		//Måste jag ha en koll om det finns något öht?

		return _context.Genres.Where(findFunc);
	}

	public async Task<Genre> AddItem(Genre item)
	{
		var result = await _context.Genres.AddAsync(item);

		return result.Entity;
	}

	public async Task<string> Delete(string id)
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
}