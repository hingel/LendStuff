using BoardGame.API.Extensions;
using BoardGame.DataAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<BoardGameDbContext>(options =>
	options.UseSqlServer(connectionString));

builder.Services.AddMediatR(o => o.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();



app.MapGet("/", () => "Hello World!");

app.MapBoardGameEndPoints();

app.Run();
