﻿using LendStuff.DataAccess.Data;
using LendStuff.DataAccess.Models;
using LendStuff.Shared;
using Microsoft.EntityFrameworkCore;

namespace LendStuff.DataAccess.Repositories;

public class UserRepository : IRepository<ApplicationUser>
{
	private readonly ApplicationDbContext _context;
	public UserRepository(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<IEnumerable<ApplicationUser>> GetAll()
	{
		return await _context.Users.ToArrayAsync();
	}

	public async Task<IEnumerable<ApplicationUser>> FindByKey(Func<ApplicationUser, bool> findFunc)
	{
		var result = _context.Users
			.Include(u => u.CollectionOfBoardGameIds)
			.Where(findFunc);
		
		return result;
	}


	//Denna metod ligger egentligen i ApplicationUserService
	public async Task<ApplicationUser> AddItem(ApplicationUser item)
	{
		throw new NotImplementedException(); ;
	}

	public Task<string> Delete(Guid id)
	{
		throw new NotImplementedException();
	}

	public async Task<ApplicationUser> Update(ApplicationUser item)
	{

		//TODO: Får fixa detta snyggare på något sätt. Blir inte så bra sätt att göra det på ändå. Blir bara mer operationer jämfört med att ta in en DTO och konvertera denna.
		//TODO: Skulle behöva Automappa detta istället.
		//var toUpdate = await _context.Users
		//	.Include(u => u.CollectionOfBoardGames)
		//	.Include(u => u.Messages)
		//	.FirstOrDefaultAsync(b => b.Email == item.Email);

		////TODO: Kanske skulle ha en del som kollar om ett värdet behöver uppdateras eller ej för att ej gå igenom samtliga objekt.
		////För user finns det många som inte bör mixtras med.

		//if (toUpdate != null)
		//{
		//	var propertyList = typeof(ApplicationUser).GetProperties();

		//	foreach (var prop in propertyList)
		//	{
		//		if (prop.GetValue(item) is null) //TODO: Vet inte om denna bidrar längre?
		//			continue;

		//		if (!prop.GetValue(item).Equals(prop.GetValue(toUpdate)))
		//		{
		//			prop.SetValue(toUpdate, prop.GetValue(item));
		//		}
		//	}

		var result = await _context.SaveChangesAsync();
		//}

		return item;
	}
}