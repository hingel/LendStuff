using BoardGame.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace BoardGame.DataAccess;

public class BoardGameDbContext : DbContext
{
	public DbSet<Models.BoardGame> BoardGames { get; set; } = null!;
	public DbSet<Genre> Genres { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Models.BoardGame>().HasKey(p => p.Id);
		
		
		//etc...
	}
}