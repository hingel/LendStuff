using Microsoft.EntityFrameworkCore;

namespace Order.DataAccess;

public class OrderDbContext : DbContext
{
	public DbSet<Models.Order> Orders { get; set; } = null!;
}