using Duende.IdentityServer.EntityFramework.Options;
using LendStuff.DataAccess.Models;
using LendStuff.Server.Models;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;

namespace LendStuff.DataAccess
{
	public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
	{
		public DbSet<BoardGame> BoardGames { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<InternalMessage> InternalMessages { get; set; }
		public DbSet<Genre> Genres { get; set; }


		public ApplicationDbContext(
			DbContextOptions options,
			IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
		{
		}
	}
}