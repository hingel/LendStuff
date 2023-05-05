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
		return _context.BoardGames;
	}

	public async Task<IEnumerable<BoardGame>> FindByKey(Func<BoardGame, bool> findFunc)
	{
		var result = _context.BoardGames.Where(findFunc);

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

	public async Task<BoardGame> Update(BoardGame item)
	{
		//TODO: Detta kanske skulle vara en egen metod som kan kallas på av flera olika repositories. I en service.

		//objektet som ska uppdateras:
		var toUpdate = await _context.BoardGames.FirstOrDefaultAsync(b => b.Id == item.Id);

		if (toUpdate != null)
		{
			var propertyList = typeof(BoardGame).GetProperties();

			foreach (var prop in propertyList)
			{
				if(prop.GetValue(item) is null) //TODO: Vet inte om denna bidrar längre?
					continue;

				if (!prop.GetValue(item).Equals(prop.GetValue(toUpdate))) //TODO: funkar inte om ett värde är null
				{
					prop.SetValue(toUpdate, prop.GetValue(item));
				}
			}
		}

		return toUpdate;
	}
}