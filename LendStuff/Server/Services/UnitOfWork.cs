using LendStuff.DataAccess.Models;
using LendStuff.DataAccess.Repositories;
using LendStuff.DataAccess.Repositories.Interfaces;
using LendStuff.Server.Models;

namespace LendStuff.DataAccess.Services;

public class UnitOfWork : IDisposable
{
	private IRepository<BoardGame> _boardGameRepository;
	private IRepository<Genre> _genreRepository;
	private readonly ApplicationDbContext _context;
	
	public UnitOfWork(ApplicationDbContext context)
	{
		_context = context; //TODO: Detta kommer från dependencyinjection, vet inte om det är rätt.
		//Vill inte skapa en ny om inte all kommunikation går genom denna?? 
	}

	public IRepository<BoardGame> BoardGameRepository
	{
		get
		{
			if (_boardGameRepository == null)
			{
				_boardGameRepository = new BoardGameRepository(_context);
			}
			return _boardGameRepository;
		}
	}

	public IRepository<Genre> GenreRepository
	{
		get
		{
			if (_genreRepository == null)
			{
				_genreRepository = new GenreRepository(_context);
			}
			return _genreRepository;
		}
	}

	public async Task<int> SaveChanges()
	{
		return await _context.SaveChangesAsync();
	}


	//TODO: Fråga niklas om Dispose
	//Ska repositories ärva IDispose också?
	//Behövs den om Unit of Work är injectad?
	public void Dispose()
	{
		_context.Dispose();
	}
}