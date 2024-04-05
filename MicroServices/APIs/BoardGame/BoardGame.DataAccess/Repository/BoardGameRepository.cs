using LendStuff.Shared;
using Microsoft.EntityFrameworkCore;

namespace BoardGame.DataAccess.Repository;

public class BoardGameRepository(BoardGameDbContext context) : IBoardGameRepository
{
    public async Task<IEnumerable<Models.BoardGame>> GetAll()
	{
		return await context.BoardGames.ToArrayAsync();
	}

    public async Task<Models.BoardGame?> GetById(Guid id)
    {
        return await context.BoardGames.Include(b => b.Genres).FirstOrDefaultAsync(b => b.Id == id);
    }
	
	public async Task<Models.BoardGame> AddItem(Models.BoardGame item)
	{
		var result = await context.BoardGames.AddAsync(item);

		return result.Entity;
	}
	
	public async Task<string> Delete(Guid id)
	{
		var result = await context.BoardGames.FirstOrDefaultAsync(b => b.Id == id);

		var test = context.BoardGames.Remove(result);

		//TODO:Mer checkar här:
		return $"{test.Entity.Title} {test.Entity.Id} removed";
	}

	public async Task<Models.BoardGame?> Update(Models.BoardGame item)
	{
		//objektet som ska uppdateras:
		var toUpdate = await context.BoardGames.FirstOrDefaultAsync(b => b.Id == item.Id);
		
		if (toUpdate != null)
		{
			var propertyList = typeof(Models.BoardGame).GetProperties();

			foreach (var prop in propertyList)
			{
				if(prop.GetValue(item) is null)
					continue;

				if (prop.GetValue(item) != null && !prop.GetValue(item)!.Equals(prop.GetValue(toUpdate)))
				{
					prop.SetValue(toUpdate, prop.GetValue(item));
				}
			}
		}

		return toUpdate;
	}

    public async Task<IEnumerable<Models.BoardGame>> GetByUserId(Guid userId)
    {
        return await context.BoardGames.Include(b => b.Genres).Where(b => b.OwnerId == userId).ToArrayAsync();
    }

    public async Task<IEnumerable<Models.BoardGame>> GetByTitle(string searchWord)
    {
        var searchWords = searchWord.Split([' ', ',', '.']);

        return await context.BoardGames.Where(b => searchWords.Contains(b.Title.ToLower())).ToArrayAsync();
    }
}