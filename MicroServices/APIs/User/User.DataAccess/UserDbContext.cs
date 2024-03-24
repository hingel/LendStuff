using Microsoft.EntityFrameworkCore;

namespace User.DataAccess;

public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
{
    public DbSet<Models.User> Users = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Models.User>().HasKey(p => p.Id);
        modelBuilder.Entity<Models.User>().Property(p => p.Name).HasMaxLength(100).IsRequired();

        base.OnModelCreating(modelBuilder);
    }
}