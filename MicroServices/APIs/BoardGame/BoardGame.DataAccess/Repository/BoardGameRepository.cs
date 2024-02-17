using LendStuff.Shared;
using Microsoft.EntityFrameworkCore;

namespace BoardGame.DataAccess.Repository;

public class BoardGameRepository: IRepository<Models.BoardGame>
{
	private readonly BoardGameDbContext _context;

	public BoardGameRepository(BoardGameDbContext context)
	{
		_context = context;
	}

	public async Task<IEnumerable<Models.BoardGame>> GetAll()
	{
		return await _context.BoardGames.ToArrayAsync();
	}

	public async Task<IEnumerable<Models.BoardGame>> FindByKey(Func<Models.BoardGame, bool> findFunc)
	{
		var result = _context.BoardGames.Include(b => b.Genres).Where(findFunc);

		return result;
	}

	public async Task<Models.BoardGame> AddItem(Models.BoardGame item)
	{
		var result = await _context.BoardGames.AddAsync(item);

		return result.Entity;
	}
	
	public async Task<string> Delete(Guid id)
	{
		var result = await _context.BoardGames.FirstOrDefaultAsync(b => b.Id == id);

		var test = _context.BoardGames.Remove(result);

		//TODO:Mer checkar här:
		return $"{test.Entity.Title} {test.Entity.Id} removed";
	}

	public async Task<Models.BoardGame?> Update(Models.BoardGame item)
	{
		//objektet som ska uppdateras:
		var toUpdate = await _context.BoardGames.FirstOrDefaultAsync(b => b.Id == item.Id);
		
		if (toUpdate != null)
		{
			var propertyList = typeof(Models.BoardGame).GetProperties();

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