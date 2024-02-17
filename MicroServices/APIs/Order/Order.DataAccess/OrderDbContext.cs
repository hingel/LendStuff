using Microsoft.EntityFrameworkCore;

namespace Order.DataAccess;

public class OrderDbContext(DbContextOptions<OrderDbContext> options) : DbContext(options)
{
	public DbSet<Models.Order> Orders { get; set; } = null!;
}