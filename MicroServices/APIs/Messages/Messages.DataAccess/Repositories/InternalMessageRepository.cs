using LendStuff.Shared;
using Messages.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Messages.DataAccess.Repositories;

public class InternalMessageRepository(MessageDbContext context) : IMessageRepository
{
	public async Task<IEnumerable<InternalMessage>> GetAll()
	{
		return await context.InternalMessages.ToArrayAsync();
	}

    public async Task<InternalMessage?> GetById(Guid id)
    {
        return await context.InternalMessages.FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<InternalMessage>> GetByUserId(Guid userId)
    {
        return await context.InternalMessages.Where(m => m.SentFromUserId == userId || m.SentToUserId == userId)
            .ToArrayAsync();
    }
    public async Task<InternalMessage> AddItem(InternalMessage item)
	{
		var result = await context.InternalMessages.AddAsync(item);

		await context.SaveChangesAsync();

		return result.Entity;
	}
	
	public async Task<string> Delete(Guid id)
	{
		var toDelete = await context.InternalMessages.FirstOrDefaultAsync(m => m.Id == id);

		if (toDelete is not null)
		{
			var result = context.InternalMessages.Remove(toDelete);
			await context.SaveChangesAsync();
			return $"Message with ID: {result.Entity.Id} deleted";
		}

		return $"Message with ID: {id} not found";
	}

	public async Task<InternalMessage?> Update(InternalMessage item)
	{
		var toUpdate = await context.InternalMessages.FirstOrDefaultAsync(m => m.Id == item.Id); //Måste jag även inkludera  properties klasser?

        if (toUpdate is null) return null;

        var listOfProperties = typeof(InternalMessage).GetProperties();

        foreach (var property in listOfProperties)
        {
            if (!property.GetValue(item)!.Equals(property.GetValue(toUpdate)))
            {
                property.SetValue(toUpdate, property.GetValue(item));
            }
        }

        await context.SaveChangesAsync();

        return toUpdate;
    }
}