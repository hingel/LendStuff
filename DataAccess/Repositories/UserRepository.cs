using LendStuff.DataAccess.Repositories.Interfaces;
using LendStuff.Server.Models;
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
		return _context.Users;
	}

	public async Task<IEnumerable<ApplicationUser>> FindByKey(Func<ApplicationUser, bool> findFunc)
	{
		var result = _context.Users
			.Include(u => u.CollectionOfBoardGames)
			.Include(u => u.Messages)
			.Where(findFunc);
		
		return result;
	}


	//Denna metod ligger egentligen i ApplicationUserService
	public async Task<ApplicationUser> AddItem(ApplicationUser item)
	{
		throw new NotImplementedException(); ;
	}

	//Denna metod ligger egentligen i ApplicationUserService
	public async Task<string> Delete(string id)
	{
		throw new NotImplementedException();
	}

	public async Task<ApplicationUser> Update(ApplicationUser item)
	{
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

			await _context.SaveChangesAsync();
		//}

		return item;
	}
}