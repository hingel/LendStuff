using LendStuff.DataAccess.Models;
using LendStuff.DataAccess.Repositories.Interfaces;
using LendStuff.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace LendStuff.DataAccess.Repositories;

public class GenreRepository : IRepository<Genre>
{
	private readonly ApplicationDbContext _context;
	public GenreRepository(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<IEnumerable<Genre>> GetAll()
	{
		return _context.Genres;
	}

	public async Task<IEnumerable<Genre>> FindByKey(Func<Genre, bool> findFunc)
	{
		//Måste jag ha en koll om det finns något öht?

		return _context.Genres.Where(findFunc);
	}

	public async Task<Genre> AddItem(Genre item)
	{
		var result = await _context.Genres.AddAsync(item);

		await _context.SaveChangesAsync();

		return result.Entity;
	}

	public async Task<string> Delete(string id)
	{
		var toDelete = await _context.Genres.FirstOrDefaultAsync(g => g.Id.Equals(id));

		if (toDelete != null)
		{
			var result = _context.Genres.Remove(toDelete);

			await _context.SaveChangesAsync();

			return $"Genre: {result.Entity.Name} deleted";
		}

		return "Genre not found.";
	}

	public async Task<Genre> Update(Genre item)
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

			await _context.SaveChangesAsync();
		}

		return toUpdate;
	}
}