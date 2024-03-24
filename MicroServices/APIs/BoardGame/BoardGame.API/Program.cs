using BoardGame.API.Extensions;
using BoardGame.API.Helpers;
using BoardGame.DataAccess;
using BoardGame.DataAccess.Models;
using BoardGame.DataAccess.Repository;
using LendStuff.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}/";
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidAudience = builder.Configuration["Auth0:Audience"],
        ValidIssuer = $"https://{builder.Configuration["Auth0:Domain"]}/",
        ValidateLifetime = true
    };
});

var host = Environment.GetEnvironmentVariable("DB_HOST");
var database = Environment.GetEnvironmentVariable("DB_DATABASE");
var username = Environment.GetEnvironmentVariable("DB_USER");
var password = Environment.GetEnvironmentVariable("DB_MSSQL_SA_PASSWORD");

var connectionString = $"Data Source={host};Initial Catalog={database};User ID={username};Password={password};Trusted_connection=False;TrustServerCertificate=True;";

builder.Services.AddDbContext<BoardGameDbContext>(options =>
	options.UseSqlServer(connectionString));

builder.Services.AddMediatR(o => o.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddScoped<IRepository<BoardGame.DataAccess.Models.BoardGame>, BoardGameRepository>();
builder.Services.AddScoped<IRepository<Genre>, GenreRepository>();
builder.Services.AddScoped<UnitOfWork>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	using (var scope = app.Services.CreateScope())
	{
		var context = scope.ServiceProvider.GetRequiredService<BoardGameDbContext>();
		context.Database.Migrate();
	}
}

app.UseAuthentication();
app.UseAuthorization();

app.MapBoardGameEndPoints();

app.Run();
