﻿using Duende.IdentityServer.EntityFramework.Options;
using LendStuff.DataAccess.Models;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace LendStuff.DataAccess.Data
{
	public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
	{

		public ApplicationDbContext(
			DbContextOptions options,
			IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
		{
		}
	}
}