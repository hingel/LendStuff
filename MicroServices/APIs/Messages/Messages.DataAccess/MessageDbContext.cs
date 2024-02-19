using Messages.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Messages.DataAccess;

public class MessageDbContext(DbContextOptions<MessageDbContext> options) : DbContext(options)
{
	public DbSet<InternalMessage> InternalMessages { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<InternalMessage>().HasKey(p => p.Id);
	}
}