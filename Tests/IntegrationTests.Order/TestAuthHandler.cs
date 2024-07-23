using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IntegrationTests.Order;

public class TestAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string SchemaName = "TestScheme";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[] { new Claim(ClaimTypes.Name, "Test User"), new Claim("Policy", "Test") };
        var identity = new ClaimsIdentity(claims, SchemaName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemaName);

        var result = AuthenticateResult.Success(ticket);
        return Task.FromResult(result);
    }

    //TODO: Testa att lägga till: https://stackoverflow.com/questions/72806554/asp-net-integration-test-override-authentication-still-calling-other-auth-handle policy istället,
    //kan inte få igenom detta med FastEndpoints som kollar schema namn. Inte Om IsAuthoized enbart gissar jag.
}