using Duende.IdentityServer.EntityFramework.Options;
using LendStuff.DataAccess.Models;
using LendStuff.Server.Models;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace LendStuff.DataAccess
{
	public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
	{
		public DbSet<Order> Orders { get; set; }
		public DbSet<InternalMessage> InternalMessages { get; set; }


		public ApplicationDbContext(
			DbContextOptions options,
			IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
		{
		}
	}
}