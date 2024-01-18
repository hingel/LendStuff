using LendStuff.DataAccess.Repositories.Interfaces;
using LendStuff.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace LendStuff.DataAccess.Repositories;

public class BoardGameRepository: IRepository<BoardGame>
{
	private readonly ApplicationDbContext _context;

	public BoardGameRepository(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<IEnumerable<BoardGame>> GetAll()
	{
		return await _context.BoardGames.ToArrayAsync();
	}

	public async Task<IEnumerable<BoardGame>> FindByKey(Func<BoardGame, bool> findFunc)
	{
		var result = _context.BoardGames.Include(b => b.Genres).Where(findFunc);

		return result;
	}

	public async Task<BoardGame> AddItem(BoardGame item)
	{
		var result = await _context.BoardGames.AddAsync(item);

		return result.Entity;
	}

	public async Task<string> Delete(string id)
	{
		var result = await _context.BoardGames.FirstOrDefaultAsync(b => b.Id == id);

		var test = _context.BoardGames.Remove(result);

		//TODO:Mer checkar här:
		return $"{test.Entity.Title} {test.Entity.Id} removed";
	}

	public async Task<BoardGame?> Update(BoardGame item)
	{
		//objektet som ska uppdateras:
		var toUpdate = await _context.BoardGames.FirstOrDefaultAsync(b => b.Id == item.Id);
		
		if (toUpdate != null)
		{
			var propertyList = typeof(BoardGame).GetProperties();

			foreach (var prop in propertyList)
			{
				if(prop.GetValue(item) is null)
					continue;

				if (!prop.GetValue(item).Equals(prop.GetValue(toUpdate)))
				{
					prop.SetValue(toUpdate, prop.GetValue(item));
				}
			}
		}

		return toUpdate;
	}
}