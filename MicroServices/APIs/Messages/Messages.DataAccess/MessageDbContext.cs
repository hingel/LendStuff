using Messages.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Messages.DataAccess;

public class MessageDbContext : DbContext
{
	public DbSet<InternalMessage> InternalMessages { get; set; } = null!;
}