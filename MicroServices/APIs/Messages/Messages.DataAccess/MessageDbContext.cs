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
		modelBuilder.Entity<InternalMessage>(m => m.Property(p => p.Message).HasMaxLength(1000).IsRequired());
		modelBuilder.Entity<InternalMessage>(m => m.Property(p => p.SentFromUserId).IsRequired());
		modelBuilder.Entity<InternalMessage>(m => m.Property(p => p.SentToUserId).IsRequired());
	}
}