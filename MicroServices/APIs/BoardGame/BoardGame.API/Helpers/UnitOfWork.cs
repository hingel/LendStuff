using BoardGame.DataAccess;
using BoardGame.DataAccess.Repository;

namespace BoardGame.API.Helpers;

public class UnitOfWork : IDisposable
{
	private IBoardGameRepository _boardGameRepository;
	private IGenreRepository _genreRepository;
	private readonly BoardGameDbContext _context;
	
	public UnitOfWork(BoardGameDbContext context)
	{
		_context = context;
	}

	public IBoardGameRepository BoardGameRepository
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

	public IGenreRepository GenreRepository
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

	public void Dispose()
	{
		_context.Dispose();
	}
}