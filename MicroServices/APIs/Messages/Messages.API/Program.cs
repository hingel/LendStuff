using LendStuff.Shared;
using Messages.API.Extensions;
using Messages.DataAccess;
using Messages.DataAccess.Models;
using Messages.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

var host = Environment.GetEnvironmentVariable("DB_HOST");
var database = Environment.GetEnvironmentVariable("DB_DATABASE");
var username = Environment.GetEnvironmentVariable("DB_USER");
var password = Environment.GetEnvironmentVariable("DB_MSSQL_SA_PASSWORD");

//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
var connectionString = $"Data Source={host};Initial Catalog={database};User ID={username};Password={password};Trusted_connection=False;TrustServerCertificate=True;";

builder.Services.AddDbContext<MessageDbContext>(options =>
	options.UseSqlServer(connectionString));

builder.Services.AddMediatR(o => o.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddScoped<IRepository<InternalMessage>, InternalMessageRepository>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	using (var scope = app.Services.CreateScope())
	{
		var context = scope.ServiceProvider.GetRequiredService<MessageDbContext>();
		context.Database.Migrate();
	}
}

app.MapMessageEndPoints();

app.Run();
