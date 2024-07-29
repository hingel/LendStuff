using Microsoft.EntityFrameworkCore;

namespace Order.DataAccess;

public class OrderDbContext(DbContextOptions<OrderDbContext> options) : DbContext(options)
{
	public DbSet<Models.Order> Orders { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Models.Order>().HasKey(o => o.OrderId);
		modelBuilder.Entity<Models.Order>(o => o.Property(p => p.BoardGameId).IsRequired());
		modelBuilder.Entity<Models.Order>(o => o.Property(p => p.OrderId).IsRequired());
		modelBuilder.Entity<Models.Order>(o => o.Property(p => p.BorrowerId).IsRequired());
		modelBuilder.Entity<Models.Order>(o => o.Property(p => p.OwnerId).IsRequired());
		modelBuilder.Entity<Models.Order>(o => o.Property(p => p.LentDate).IsRequired());
		modelBuilder.Entity<Models.Order>(o => o.Property(p => p.ReturnDate).IsRequired());
		modelBuilder.Entity<Models.Order>(o => o.Property(p => p.Status).IsRequired());
		modelBuilder.Entity<Models.Order>(o => o.Property(p => p.OrderMessageGuids).IsRequired());
	}
}