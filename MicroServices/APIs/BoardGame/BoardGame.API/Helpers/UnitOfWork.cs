using BoardGame.DataAccess;
using BoardGame.DataAccess.Repository;

namespace BoardGame.API.Helpers;

public class UnitOfWork(BoardGameDbContext context) : IDisposable
{
	private IBoardGameRepository? _boardGameRepository;
	private IGenreRepository? _genreRepository;

    public IBoardGameRepository BoardGameRepository
	{
		get
		{
			if (_boardGameRepository == null)
			{
				_boardGameRepository = new BoardGameRepository(context);
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
				_genreRepository = new GenreRepository(context);
			}
			return _genreRepository;
		}
	}

	public async Task<int> SaveChanges()
	{
		return await context.SaveChangesAsync();
	}

	public void Dispose()
	{
		context.Dispose();
	}
}