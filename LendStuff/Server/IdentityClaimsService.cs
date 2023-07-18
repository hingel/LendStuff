using System.Security.Claims;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using LendStuff.Server.Models;
using Microsoft.AspNetCore.Identity;

namespace LendStuff.Server;

public class IdentityClaimsService : IProfileService
{
	private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
	private readonly UserManager<ApplicationUser> _userManager;


	public IdentityClaimsService(IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
		UserManager<ApplicationUser> userManager)
	{
		_claimsFactory = claimsFactory;
		_userManager = userManager;
	}

	public async Task GetProfileDataAsync(ProfileDataRequestContext context)
	{
		var userId = context.Subject.GetSubjectId();
		var user = await _userManager.FindByIdAsync(userId);
		var claimsPrincipal = await _claimsFactory.CreateAsync(user);
		var claims = claimsPrincipal.Claims.ToList();

		var userClaimsDb = await _userManager.GetClaimsAsync(user);
		var mappedClaims = new List<Claim>();

		foreach (var claim in userClaimsDb)
		{
			if (claim.Type == ClaimTypes.Role)
			{
				mappedClaims.Add(new Claim(JwtClaimTypes.Role, claim.Value));
			}
			else
			{
				mappedClaims.Add(claim);
			}
		}

		claims.AddRange(mappedClaims);
		context.IssuedClaims = claims;
	}

	public async Task IsActiveAsync(IsActiveContext context)
	{
		var userId = context.Subject.GetSubjectId();
		var user = _userManager.FindByIdAsync(userId);
		context.IsActive = user != null;
	}
}