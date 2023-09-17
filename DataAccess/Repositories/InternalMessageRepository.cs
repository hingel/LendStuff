using System.Reflection;
using LendStuff.DataAccess.Models;
using LendStuff.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LendStuff.DataAccess.Repositories;

public class InternalMessageRepository : IRepository<InternalMessage>
{
	private readonly ApplicationDbContext _context;

	public InternalMessageRepository(ApplicationDbContext context)
	{
		_context = context;
	}


	//Denna metoden borde ju typ aldrig användas?!
	public async Task<IEnumerable<InternalMessage>> GetAll()
	{
		return await _context.InternalMessages.ToArrayAsync();
	}

	public async Task<IEnumerable<InternalMessage>> FindByKey(Func<InternalMessage, bool> findFunc)
	{
		return _context.InternalMessages
			.Include(m => m.SentToUser)
			.Where(findFunc);
	}

	public async Task<InternalMessage> AddItem(InternalMessage item)
	{
		var result = await _context.InternalMessages.AddAsync(item);

		await _context.SaveChangesAsync();

		return result.Entity;
	}

	public async Task<string> Delete(string id)
	{
		var toDelete = await _context.InternalMessages.FirstOrDefaultAsync(m => m.MessageId.ToString() == id);

		if (toDelete is not null)
		{
			var result = _context.InternalMessages.Remove(toDelete);
			await _context.SaveChangesAsync();
			return $"Message with ID: {result.Entity.MessageId} deleted";
		}

		return $"Message with ID: {id} not found";
	}

	public async Task<InternalMessage?> Update(InternalMessage item)
	{
		var toUpdate = await _context.InternalMessages.FirstOrDefaultAsync(m => m.MessageId == item.MessageId); //Måste jag även inkludera  properties klasser?

		if (toUpdate is not null)
		{
			var listOfProperties = typeof(InternalMessage).GetProperties();

			foreach (var property in listOfProperties)
			{
				if (!property.GetValue(item)!.Equals(property.GetValue(toUpdate)))
				{
					property.SetValue(toUpdate, property.GetValue(item));
				}
			}

			await _context.SaveChangesAsync();

			return toUpdate;

		}

		return null;

	}

}