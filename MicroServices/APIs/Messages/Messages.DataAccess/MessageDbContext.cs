using Messages.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Messages.DataAccess;

public class MessageDbContext(DbContextOptions<MessageDbContext> options) : DbContext(options)
{
	public DbSet<InternalMessage> InternalMessages { get; set; } = null!;
}