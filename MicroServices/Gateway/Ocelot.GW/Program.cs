using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("ocelot.json")
    .AddEnvironmentVariables();

var authenticationProviderKey = "MyKey";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(authenticationProviderKey ,options =>
{
    options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}/";
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidAudience = builder.Configuration["Auth0:Audience"],
        ValidIssuer = $"https://{builder.Configuration["Auth0:Domain"]}/",
        ValidateLifetime = true
    };
});

builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

app.UseOcelot().Wait();

app.Run();
