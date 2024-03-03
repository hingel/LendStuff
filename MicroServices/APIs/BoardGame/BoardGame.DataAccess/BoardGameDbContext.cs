using BoardGame.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace BoardGame.DataAccess;

public class BoardGameDbContext(DbContextOptions<BoardGameDbContext> options) : DbContext(options)
{
	public DbSet<UserBoardGame> UserBoardGames { get; set; } = null!;
	public DbSet<Models.BoardGame> BoardGames { get; set; } = null!;
	public DbSet<Genre> Genres { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<UserBoardGame>().HasKey(p => p.Id);
		modelBuilder.Entity<UserBoardGame>().Property(p => p.Comment).HasMaxLength(500).IsRequired();
		modelBuilder.Entity<UserBoardGame>().Property(p => p.Condition).IsRequired();
		modelBuilder.Entity<UserBoardGame>().Property(p => p.ForLending).IsRequired();
		modelBuilder.Entity<UserBoardGame>().Property(p => p.Owner).IsRequired();

		modelBuilder.Entity<UserBoardGame>().HasOne(p => p.BoardGame).WithMany().IsRequired();

		modelBuilder.Entity<Models.BoardGame>().HasKey(p => p.Id);
		modelBuilder.Entity<Models.BoardGame>().Property(p => p.Title).HasMaxLength(500).IsRequired();
		modelBuilder.Entity<Models.BoardGame>().Property(p => p.ReleaseYear).IsRequired();
		modelBuilder.Entity<Models.BoardGame>().Property(p => p.Description).HasMaxLength(500).IsRequired();
		modelBuilder.Entity<Models.BoardGame>().Property(p => p.BggLink).HasMaxLength(250);

		modelBuilder.Entity<Models.BoardGame>().HasMany(p => p.Genres).WithMany();

		modelBuilder.Entity<Genre>().HasKey(p => p.Id);
		modelBuilder.Entity<Genre>().Property(p => p.Name).HasMaxLength(20).IsRequired();
		
		
		base.OnModelCreating(modelBuilder);
	}
}