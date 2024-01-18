using BoardGame.DataAccess;
using BoardGame.DataAccess.Models;
using BoardGame.DataAccess.Repository;

namespace BoardGame.API.Helpers;

public class UnitOfWork : IDisposable
{
	private IRepository<BoardGame.DataAccess.Models.BoardGame> _boardGameRepository;
	private IRepository<Genre> _genreRepository;
	private readonly BoardGameDbContext _context;
	
	public UnitOfWork(BoardGameDbContext context)
	{
		_context = context;
	}

	public IRepository<BoardGame.DataAccess.Models.BoardGame> BoardGameRepository
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